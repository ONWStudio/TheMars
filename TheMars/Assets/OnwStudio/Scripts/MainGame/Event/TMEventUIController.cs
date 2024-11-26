using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using Onw.Coroutine;
using Onw.Attribute;
using Onw.Extensions;
using Onw.UI.Components;
using TM.Cost;
using TM.Event;
using TM.Event.Effect;
using System.Text;

namespace TM.UI
{
    public sealed class TMEventUIController : MonoBehaviour
    {
        public struct TMDescriptionCallbackPair : IDisposable
        {
            public LocalizedString LocalizedDescription { get; private set; }
            public LocalizedString.ChangeHandler ChangeHandler { get; private set; }

            public void Dispose()
            {
                if (ChangeHandler is not null) 
                {
                    LocalizedDescription.StringChanged -= ChangeHandler;
                }

                LocalizedDescription = null;
            }

            public TMDescriptionCallbackPair(LocalizedString localizedDescription, LocalizedString.ChangeHandler changeHandler)
            {
                LocalizedDescription = localizedDescription;
                ChangeHandler = changeHandler;

                if (ChangeHandler is not null)
                {
                    LocalizedDescription.StringChanged += ChangeHandler;
                }
            }
        }

        [Header("Canvas")]
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;

        [Header("Button")]
        [SerializeField, SelectableSerializeField] private Button _topButton;
        [SerializeField, SelectableSerializeField] private Button _bottomButton;

        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _eventImage;

        [Header("Text")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _topButtonText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _bottomButtonText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _titleText;

        [Header("Localization Event")]
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _descriptionTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _topButtonTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _bottomButtonTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _titleTextEvent;

        [Header("Triggers")]
        [SerializeField, SelectableSerializeField] private PointerEnterTrigger _topButtonEnterTrigger;
        [SerializeField, SelectableSerializeField] private PointerEnterTrigger _bottomButtonEnterTrigger;
        [SerializeField, SelectableSerializeField] private PointerExitTrigger _topButtonExitTrigger;
        [SerializeField, SelectableSerializeField] private PointerExitTrigger _bottomButtonExitTrigger;

        private string _description = string.Empty;
        private string _topEffectDescription = string.Empty;
        private string _bottomEffectDescription = string.Empty;
        private ITMEventRunner _eventRunner = null;
        private readonly List<TMDescriptionCallbackPair> _topEffectPairs = new();
        private readonly List<TMDescriptionCallbackPair> _bottomEffectPairs = new();

        [SerializeField, LocalizedString(tableName: "TM_UI", entryKey: "PaymentHeader")] private LocalizedString _paymentDescriptionHeader;
        [SerializeField, LocalizedString(tableName: "TM_UI", entryKey: "EffectHeader")] private LocalizedString _eventEffectHeader;

        private void Awake()
        {
            _topButton.onClick.AddListener(onClickTopButton);
            _bottomButton.onClick.AddListener(onClickBottomButton);

            _topButtonEnterTrigger.OnPointerEnterEvent += onPointerEnterByTop;
            _bottomButtonEnterTrigger.OnPointerEnterEvent += onPointerEnterByBottom;
            _topButtonExitTrigger.OnPointerExitEvent += onPointerExitByTop;
            _bottomButtonExitTrigger.OnPointerExitEvent += onPointerExitByBottom;

            _descriptionTextEvent.OnUpdateString.AddListener(description => _descriptionText.text = _description = description);
            _topButtonTextEvent.OnUpdateString.AddListener(topText => _topButtonText.text = topText);
            _bottomButtonTextEvent.OnUpdateString.AddListener(bottomText => _bottomButtonText.text = bottomText);
            _titleTextEvent.OnUpdateString.AddListener(titleText => _titleText.text = titleText);
        }

        public void SetActiveEventUI(bool isActive)
        {
            _canvas.enabled = isActive;
        }

        private void onPointerEnterByTop(PointerEventData eventData)
        {
            _descriptionText.text = _topEffectDescription;
        }

        private void onPointerEnterByBottom(PointerEventData eventData)
        {
            _descriptionText.text = _bottomEffectDescription;
        }

        private void onPointerExitByTop(PointerEventData eventData)
        {
            _descriptionText.text = _description;
        }

        private void onPointerExitByBottom(PointerEventData eventData)
        {
            _descriptionText.text = _description;
        }

        private void onClickTopButton()
        {
            if (!_eventRunner.CanFireTop) return;
            onEffect(TMEventChoice.TOP);
        }

        private void onClickBottomButton()
        {
            if (!_eventRunner.CanFireBottom) return;
            onEffect(TMEventChoice.BOTTOM);
        }

        // TODO : 이벤트 호출 순서가 맞지않음
        private void onEffect(TMEventChoice choice)
        {
            SetActiveEventUI(false);
            _topEffectPairs.ForEach(pair => pair.Dispose());
            _topEffectPairs.Clear();
            _bottomEffectPairs.ForEach(pair => pair.Dispose());
            _bottomEffectPairs.Clear();
            _eventRunner.InvokeEvent(choice);
            resetField();
        }

        private void resetField()
        {
            _description = string.Empty;
            _topEffectDescription = string.Empty;
            _bottomEffectDescription = string.Empty;
            _eventRunner = null;
            _eventImage.sprite = null;
            _descriptionTextEvent.StringReference = null;
            _topButtonTextEvent.StringReference = null;
            _bottomButtonTextEvent.StringReference = null;
            _titleTextEvent.StringReference = null;
        }

        // TODO : 이벤트 여러개 중첩되서 발동될경우 UI 활성화 되지 않는 현상
        public void OnTriggerMainEvent(ITMEventRunner mainEventRunner)
        {
            SetActiveEventUI(true);

            _eventRunner = mainEventRunner;
            _eventImage.sprite = _eventRunner.EventReadData.EventImage;
            _titleTextEvent.StringReference = _eventRunner.EventReadData.TitleTextEvent;
            _descriptionTextEvent.StringReference = _eventRunner.EventReadData.DescriptionTextEvent;

            _topButton.SetActiveGameObject(_eventRunner.EventReadData.HasTopEvent);
            if (_eventRunner.EventReadData.HasTopEvent)
            {
                _topButtonTextEvent.StringReference = _eventRunner.EventReadData.TopButtonTextEvent;
                _topEffectPairs.AddRange(buildDescription(_eventRunner.TopEffects, _eventRunner.TopCosts, buildText => _topEffectDescription = buildText));
            }
            
            _bottomButton.SetActiveGameObject(_eventRunner.EventReadData.HasBottomEvent);
            if (_eventRunner.EventReadData.HasBottomEvent)
            {
                _bottomButtonTextEvent.StringReference = _eventRunner.EventReadData.BottomButtonTextEvent;
                _bottomEffectPairs.AddRange(buildDescription(_eventRunner.BottomEffects, _eventRunner.BottomCosts, buildText => _bottomEffectDescription = buildText));
            }

            TMDescriptionCallbackPair[] buildDescription(IReadOnlyList<ITMEventEffect> effects, IReadOnlyList<ITMCost> costs, Action<string> onBuildedDescription)
            {
                ITMEventEffect[] effectArray = effects
                    .Where(effect => effect.EffectDescription is not null)
                    .ToArray();

                ITMCost[] costArray = costs
                    .Where(cost => cost.CostDescription is not null)
                    .ToArray();

                bool isReload = false;

                return effectArray
                    .Select(effect => new TMDescriptionCallbackPair(effect.EffectDescription, _ => onBuildDescription(effectArray, costArray, onBuildedDescription)))
                    .Concat(costArray.Select(cost => new TMDescriptionCallbackPair(cost.CostDescription, _ => onBuildDescription(effectArray, costArray, onBuildedDescription))))
                    .ToArray();

                void onBuildDescription(ITMEventEffect[] effects, ITMCost[] costs, Action<string> onBuildedDescription)
                {
                    if (isReload) return;

                    isReload = true; // .. 1프레임 대기후 호출
                    this.DoCallWaitForOneFrame(() =>
                    {
                        isReload = false;

                        if (onBuildedDescription is not null)
                        {
                            StringBuilder descriptionBuilder = new();

                            if (costs.Length > 0)
                            {
                                descriptionBuilder.Append(_paymentDescriptionHeader.GetLocalizedString());
                                descriptionBuilder.Append("\n \n");
                                descriptionBuilder.Append(string.Join("\n", costs.Select(cost => cost.CostDescription.GetLocalizedString())));
                                descriptionBuilder.Append("\n \n");
                            }

                            if (effects.Length > 0)
                            {
                                descriptionBuilder.Append(_eventEffectHeader.GetLocalizedString());
                                descriptionBuilder.Append("\n \n");
                                descriptionBuilder.Append(string.Join("\n", effects.Select(effect => effect.EffectDescription.GetLocalizedString())));
                            }

                            onBuildedDescription.Invoke(descriptionBuilder.ToString());
                        }
                    });
                }
            }
        }
    }
}