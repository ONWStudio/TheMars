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
using UnityEngine.Localization;

namespace TM.Card.Effect
{
    [System.Serializable]
    public sealed class TMCardGetResourceEffect : ITMCardResourceEffect, ITMCardInitializeEffect<TMCardGetResourceEffectCreator>
    {
        public IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources => _resources;

        public bool CanUseEffect => true;

        private Dictionary<TMResourceKind, TMResourceDataForRuntime> _resources = new();

        public event LocalizedString.ChangeHandler OnChangedDescription
        {
            add => _localizedDescription.StringChanged += value;
            remove => _localizedDescription.StringChanged -= value;
        }

        [SerializeField, ReadOnly]
        private LocalizedString _localizedDescription = new("TM_Card_Effect", "Get_Resource_Effect");


        public void AddResource(TMResourceKind resourceKind, int additionalAmount)
        {
            if (!_resources.TryGetValue(resourceKind, out TMResourceDataForRuntime runtimeResource))
            {
                Debug.LogWarning($"{resourceKind} : 해당 자원 획득 효과가 존재하지 않습니다");
                return;
            }

            runtimeResource.AdditionalResources.Add(additionalAmount);
        }

        public void Initialize(TMCardGetResourceEffectCreator effectCreator)
        {
            _resources = effectCreator
                .Resources
                .ToDictionary(
                    resource => resource.ResourceKind,
                    resource => (TMResourceDataForRuntime)resource);


            _localizedDescription.Arguments = new object[] 
            { 
                _resources.Values.Select(data => new
                {
                    Kind = data.ResourceKind,
                    Resource = Mathf.Abs(data.FinalResource),
                    Positive = data.FinalResource >= 0
                }).ToList()
            };
        }

        public void Dispose() { }

        public void ApplyEffect(TMCardModel cardModel)
        {
        }

        public void OnEffect(TMCardModel cardModel)
        {
            _resources.Values.ForEach(resource
                    => TMPlayerManager.Instance.AddResource(resource.ResourceKind, resource.FinalResource));
        }
    }
}