using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.AddressableAssets;
using Onw.Scope;
using Onw.Helper;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Extensions;
using Onw.Manager.Prototype;
using Onw.Manager.ObjectPool;
using TM.Synergy;
using TM.Synergy.Effect;

namespace TM.UI
{
    public sealed class TMSynergyViewController : MonoBehaviour
    {
        public struct SynergyEffectCallbackFair : IDisposable
        {
            public TMSynergyEffect Effect { get; private set; }
            public LocalizedString.ChangeHandler ChangeHandler { get; private set; }

            public void Dispose()
            {
                Effect.LocalizedDescription.StringChanged -= ChangeHandler;
                Effect = null;
                ChangeHandler = null;
            }

            public SynergyEffectCallbackFair(TMSynergyEffect effect, LocalizedString.ChangeHandler onChangedString)
            {
                Effect = effect;
                ChangeHandler = onChangedString;
                Effect.LocalizedDescription.StringChanged += ChangeHandler;
            }
        }

        public readonly struct SynergyArguments
        {
            public TMSynergy Synergy { get; }
            public TMSynergyScrollItem ScrollItem { get; }
            public List<SynergyEffectCallbackFair> CallbackFairs { get; }

            public SynergyArguments(TMSynergy synergy, TMSynergyScrollItem scrollItem)
            {
                Synergy = synergy;
                ScrollItem = scrollItem;
                CallbackFairs = new();
            }
        }

        [SerializeField] private AssetReferenceGameObject _scrollItemPrefabReference;
        [SerializeField, SelectableSerializeField] private ScrollRect _scrollView;
        [SerializeField, SelectableSerializeField] private TMSynergyDescriptor _descriptor;

        private readonly Dictionary<string, SynergyArguments> _scrollItems = new();
        private string _currentSynergyId = string.Empty;
        private bool _popupReload = false;

        private void OnDisable()
        {
            _popupReload = false;
        }

        public void OnUpdateSynergies(TMSynergy[] synergies)
        {
            using (ListPoolScope<KeyValuePair<string, SynergyArguments>> poolScope = new())
            {
                List<KeyValuePair<string, SynergyArguments>> removeKeys = poolScope.Get();

                removeKeys.AddRange(_scrollItems
                    .Where(kvp => synergies.All(synergy => synergy.SynergyData.ID != kvp.Key)));

                foreach (KeyValuePair<string, SynergyArguments> synergyScrollItemKvp in removeKeys) // .. 삭제 로직
                {
                    if (_currentSynergyId == synergyScrollItemKvp.Value.Synergy.SynergyData.ID)
                    {
                        _currentSynergyId = string.Empty;
                        _descriptor.Description = string.Empty;
                        _descriptor.Name = string.Empty;
                        _descriptor.SetActiveDescriptor(false);
                        synergyScrollItemKvp.Value.CallbackFairs.ForEach(fair => fair.Dispose());
                    }

                    synergyScrollItemKvp.Value.Synergy.LocalizedSynergyName.StringChanged -= setDescriptorName;
                    _scrollItems.Remove(synergyScrollItemKvp.Key);
                    GenericObjectPool<TMSynergyScrollItem>.Return(synergyScrollItemKvp.Value.ScrollItem);
                }
            }

            foreach (TMSynergy synergy in synergies)
            {
                if (!_scrollItems.TryGetValue(synergy.SynergyData.ID, out SynergyArguments pair))
                {
                    if (!GenericObjectPool<TMSynergyScrollItem>.TryPop(out TMSynergyScrollItem scrollItem))
                    {
                        scrollItem = PrototypeManager.Instance.ClonePrototypeFromReferenceSync<TMSynergyScrollItem>(_scrollItemPrefabReference);
                    }

                    pair = new(synergy, scrollItem);
                    pair.ScrollItem.Initialize(synergy);
                    pair.ScrollItem.OnPointerEnterEvent += onPointerEnterEvent;
                    pair.ScrollItem.OnPointerExitEvent += onPointerExitEvent;

                    void onPointerEnterEvent()
                    {
                        _descriptor.SetActiveDescriptor(true);
                        _currentSynergyId = pair.Synergy.SynergyData.ID;
                        pair.CallbackFairs.AddRange(pair
                            .Synergy
                            .SynergyEffects
                            .Select(effect => new SynergyEffectCallbackFair(effect, _ => reloadDescription())));
                        pair.Synergy.LocalizedSynergyName.StringChanged += setDescriptorName;
                    }

                    void onPointerExitEvent()
                    {
                        _descriptor.SetActiveDescriptor(false);
                        _currentSynergyId = string.Empty;
                        pair.CallbackFairs.ForEach(callbackPair => callbackPair.Dispose());
                        pair.CallbackFairs.Clear();
                        pair.Synergy.LocalizedSynergyName.StringChanged -= setDescriptorName;
                    }
                    
                    _scrollItems.Add(pair.Synergy.SynergyData.ID, pair);
                }

                pair.ScrollItem.SetView(synergy);
                pair.ScrollItem.SetParent(_scrollView.content, false);
            }
        }

        private void reloadDescription()
        {
            if (_popupReload || string.IsNullOrEmpty(_currentSynergyId)) return;

            _popupReload = true;
            string keepId = _currentSynergyId;

            this.DoCallWaitForOneFrame(() =>
            {
                _popupReload = false;

                if (keepId == _currentSynergyId && _scrollItems.TryGetValue(_currentSynergyId, out SynergyArguments arguments))
                {
                    TMSynergy synergy = arguments.Synergy;

                    TMSynergyEffect[] sortedEffects = synergy
                        .SynergyEffects
                        .OrderBy(effect => effect.TargetBuildingCount)
                        .ToArray();

                    StringBuilder descriptionBuilder = new();

                    foreach (TMSynergyEffect effect in sortedEffects)
                    {
                        bool isEnabled = effect.TargetBuildingCount <= synergy.BuildingCount;
                        if (isEnabled)
                        {
                            descriptionBuilder.Append("(");
                            descriptionBuilder.Append(effect.TargetBuildingCount.ToString());
                            descriptionBuilder.Append(") ");
                            descriptionBuilder.Append(effect.LocalizedDescription.GetLocalizedString());
                        }
                        else
                        {
                            descriptionBuilder.Append(RichTextFormatter.Colorize("(", Color.gray));
                            descriptionBuilder.Append(RichTextFormatter.Colorize(effect.TargetBuildingCount.ToString(), Color.gray));
                            descriptionBuilder.Append(RichTextFormatter.Colorize(") ", Color.gray));
                            descriptionBuilder.Append(RichTextFormatter.Colorize(effect.LocalizedDescription.GetLocalizedString(), Color.gray));
                        }

                        descriptionBuilder.Append("\n \n");
                    }

                    _descriptor.Description = descriptionBuilder.ToString();
                }
            });
        }

        private void setDescriptorName(string synergyName)
        {
            _descriptor.Name = synergyName;
        }
    }
}