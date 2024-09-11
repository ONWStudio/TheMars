using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.Helper;
using Onw.ServiceLocator;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;
using TM.Class;
using TMPro;

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

        public event UnityAction<string> OnNotifyEvent
        {
            add => _onNotifyEvent.AddListener(value);
            remove => _onNotifyEvent.RemoveListener(value);
        }

        [SerializeField, DisplayAs("획득 재화"), Tooltip("획득 재화"), ReadOnly]
        private Dictionary<TMResourceKind, TMResourceDataForRuntime> _resources = new();

        [SerializeField, ReadOnly] private UnityEvent<string> _onNotifyEvent = new();
        [SerializeField, ReadOnly] private int _additionalAmount = 0;

        public void Initialize(TMCardGetResourceEffectCreator cardGetResourceEffectCreator)
        {
            _resources = cardGetResourceEffectCreator
                .Resources
                .ToDictionary(
                    resource => resource.ResourceKind,
                    resource => (TMResourceDataForRuntime)resource);
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            trigger.OnEffectEvent += _ =>
            {
                if (!ServiceLocator<PlayerManager>.TryGetService(out PlayerManager player)) return;

                foreach (TMResourceDataForRuntime runtimeResource in _resources.Values)
                {
                    switch (runtimeResource.ResourceKind)
                    {
                        case TMResourceKind.MARS_LITHIUM:
                            player.MarsLithium += runtimeResource.FinalResource;
                            break;
                        case TMResourceKind.CREDIT:
                            player.Credit += runtimeResource.FinalResource;
                            break;
                        case TMResourceKind.STEEL:
                            player.Steel += runtimeResource.FinalResource;
                            break;
                        case TMResourceKind.PLANTS:
                            player.Plants += runtimeResource.FinalResource;
                            break;
                        case TMResourceKind.CLAY:
                            player.Clay += runtimeResource.FinalResource;
                            break;
                        case TMResourceKind.ELECTRICITY:
                            player.Electricity += runtimeResource.FinalResource;
                            break;
                        case TMResourceKind.POPULATION:
                            player.Population += runtimeResource.FinalResource;
                            break;
                    }
                }
            };
        }

        public void Dispose() { }
    }
}