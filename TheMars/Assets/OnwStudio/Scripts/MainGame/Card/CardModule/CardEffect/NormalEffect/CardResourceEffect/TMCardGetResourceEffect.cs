using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.Extensions;
using Onw.Helper;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;
using TM.Class;

namespace TM.Card.Effect
{
    [System.Serializable]
    public sealed class TMCardGetResourceEffect : ITMCardResourceEffect, ITMCardInitializeEffect<TMCardGetResourceEffectCreator>
    {
        public string Description
        {
            get
            {
                return string
                    .Join("\n", _resources
                        .Values
                        .Select(selector));

                string selector(TMResourceDataForRuntime resource)
                {
                    string description = $"{RichTextFormatter.SpriteIcon((int)resource.ResourceKind)}{(resource.FinalResource < 0 ? resource.FinalResource.ToString() : $"+{resource.FinalResource}")}";

                    if (resource.AdditionalResource != 0)
                    {
                        description +=
                            resource.AdditionalResource > 0 ?
                                RichTextFormatter.Colorize($"+{resource.AdditionalResource}", Color.green) :
                                RichTextFormatter.Colorize(resource.AdditionalResource.ToString(), Color.red);
                    }

                    return description;
                }
            }
        }

        public IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources => _resources;

        public event UnityAction<string> OnNotifyEvent
        {
            add => _onNotifyEvent.AddListener(value);
            remove => _onNotifyEvent.RemoveListener(value);
        }

        private Dictionary<TMResourceKind, TMResourceDataForRuntime> _resources = new();

        [SerializeField, ReadOnly] private UnityEvent<string> _onNotifyEvent = new();

        public void AddResource(TMResourceKind resourceKind, int additionalAmount)
        {
            if (!_resources.TryGetValue(resourceKind, out TMResourceDataForRuntime runtimeResource))
            {
                Debug.LogWarning($"{resourceKind} : 해당 자원 획득 효과가 존재하지 않습니다");
                return;
            }

            runtimeResource.AdditionalResources.Add(additionalAmount);
            _onNotifyEvent.Invoke(Description);
        }

        public void Initialize(TMCardGetResourceEffectCreator effectCreator)
        {
            _resources = effectCreator
                .Resources
                .ToDictionary(
                    resource => resource.ResourceKind,
                    resource => (TMResourceDataForRuntime)resource);
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            trigger.OnEffectEvent += _ =>
                _resources.Values.ForEach(resource
                    => TMPlayerManager.Instance.AddResource(resource.ResourceKind, resource.FinalResource));
        }

        public void Dispose() { }
    }
}