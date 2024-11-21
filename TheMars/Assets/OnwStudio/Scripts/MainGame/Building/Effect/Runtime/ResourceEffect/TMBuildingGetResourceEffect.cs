using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using TM.Building.Effect.Creator;
using TM.Manager;
using UnityEngine.Localization;

namespace TM.Building.Effect
{
    public sealed class TMBuildingGetResourceEffect : ITMBuildingResourceEffect, ITMBuildingInitializeEffect<TMBuildingGetResourceEffectCreator>
    {
        [field: SerializeField, ReadOnly] public float RepeatSeconds { get; set; } = 0f;

        [field: SerializeField, ReadOnly] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, ReadOnly] public int AdditionalResource { get; private set; }

        [field: SerializeField, ReadOnly] public LocalizedString LocalizedDescription { get; private set; } = new("TM_Building_Effect", "Get_Resource_Effect");

        private Coroutine _coroutine = null;

        public void Initialize(TMBuildingGetResourceEffectCreator creator)
        {
            Kind = creator.Kind;
            AdditionalResource = creator.AdditionalResource;
            RepeatSeconds = creator.RepeatSeconds;

            LocalizedDescription.Arguments = new object[]
            {
                new
                {
                    RepeatSeconds,
                    Kind,
                    Resource = Mathf.Abs(AdditionalResource),
                    Positive = AdditionalResource >= 0
                }
            };
        }
        
        public void AddResource(int additionalAmount)
        {
            AdditionalResource += additionalAmount;
            
            LocalizedDescription.Arguments = new object[]
            {
                new
                {
                    RepeatSeconds,
                    Kind,
                    Resource = Mathf.Abs(AdditionalResource),
                    Positive = AdditionalResource >= 0
                }
            };
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
                    
                    timeAccumulator += Time.deltaTime * TimeManager.TimeScale;

                    if (timeAccumulator >= RepeatSeconds)
                    {
                        int divideCount = (int)(timeAccumulator / RepeatSeconds);

                        TMPlayerManager.Instance.AddResource(Kind, AdditionalResource * divideCount);
                        timeAccumulator -= RepeatSeconds * divideCount;
                    }

                    yield return null;
                }
            }
        }
    }
}
