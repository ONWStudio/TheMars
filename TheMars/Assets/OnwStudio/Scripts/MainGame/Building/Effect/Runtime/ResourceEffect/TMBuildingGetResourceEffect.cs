using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Extensions;
using Onw.ServiceLocator;
using TM.Building.Effect.Creator;
using TM.Class;
using TM.Manager;
using UnityEngine.Events;

namespace TM.Building.Effect
{
    public sealed class TMBuildingGetResourceEffect : ITMBuildingResourceEffect, ITMBuildingInitializeEffect<TMBuildingGetResourceEffectCreator>
    {
        public IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources => _resources;

        public event UnityAction<string> OnNotifyEvent
        {
            add => _onNotifyEvent.AddListener(value);
            remove => _onNotifyEvent.RemoveListener(value);
        }

        [field: SerializeField, ReadOnly] public float RepeatSeconds { get; set; } = 0f;

        [SerializeField, ReadOnly] private UnityEvent<string> _onNotifyEvent = new();
        
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
            _onNotifyEvent.Invoke("Description");
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
                    timeAccumulator += Time.deltaTime * TimeManager.GameSpeed;

                    if (timeAccumulator >= RepeatSeconds)
                    {
                        ServiceLocator<PlayerManager>
                            .InvokeService(player => _resources.Values.ForEach(resource 
                                => player.AddResource(resource.ResourceKind, resource.FinalResource)));
                        
                        timeAccumulator -= RepeatSeconds;
                    }

                    yield return null;
                }
            }
        }
    }
}
