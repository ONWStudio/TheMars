using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Extensions;
using TM.Building.Effect.Creator;
using TM.Class;
using TM.Manager;

namespace TM.Building.Effect
{
    public sealed class TMBuildingGetResourceEffect : ITMBuildingResourceEffect, ITMBuildingInitializeEffect<TMBuildingGetResourceEffectCreator>
    {
        public IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources => _resources;

        [field: SerializeField, ReadOnly] public float RepeatSeconds { get; set; } = 0f;

        private Dictionary<TMResourceKind, TMResourceDataForRuntime> _resources = new(); 
        private Coroutine _coroutine = null;

        public void Initialize(TMBuildingGetResourceEffectCreator effectCreator)
        {
            _resources = effectCreator
                .Resources
                .ToDictionary(
                    resource => resource.ResourceKind,
                    resource => (TMResourceDataForRuntime)resource);

            RepeatSeconds = effectCreator.RepeatSeconds;
        }
        
        public void AddResource(TMResourceKind resourceKind, int additionalAmount)
        {
            if (!_resources.TryGetValue(resourceKind, out TMResourceDataForRuntime runtimeResource))
            {
                Debug.LogWarning($"{resourceKind} : 해당 자원 획득 효과가 존재하지 않습니다");
                return;
            }
            
            runtimeResource.AdditionalResources.Add(additionalAmount);
        }
        
        public void ApplyEffect(TMBuilding owner)
        {
            fireEffect(owner);
        }

        public void DisableEffect(TMBuilding owner)
        {
            owner.StopCoroutineIfNotNull(_coroutine);
        }

        private void fireEffect(TMBuilding owner)
        {
            owner.StopCoroutineIfNotNull(_coroutine);
            _coroutine = owner.StartCoroutine(iEFireEffect());
            
            IEnumerator iEFireEffect()
            {
                float timeAccumulator = 0f;

                while (true)
                {
                    if (!owner.IsActive.Value) yield return null;
                    
                    timeAccumulator += Time.deltaTime * TimeManager.GameSpeed;

                    if (timeAccumulator >= RepeatSeconds)
                    {
                        _resources.Values.ForEach(resource => TMPlayerManager.Instance.AddResource(resource.ResourceKind, resource.FinalResource));
                        timeAccumulator -= RepeatSeconds;
                    }

                    yield return null;
                }
            }
        }
    }
}
