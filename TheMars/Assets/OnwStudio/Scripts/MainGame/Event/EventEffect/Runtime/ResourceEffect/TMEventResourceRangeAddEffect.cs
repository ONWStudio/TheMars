using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventResourceRangeAddEffect : TMEventResourceAddEffectBase, ITMEventInitializeEffect<TMEventResourceRangeAddEffectCreator>
    {
        [SerializeField] private LocalizedString _effectDescription = new("TM_Event_Effect", "Resource_Range_Add_Effect");

        public override LocalizedString EffectDescription => _effectDescription;

        [field: SerializeField] public int Min {get; set; }
        [field: SerializeField] public int Max { get; set; }

        public void Initialize(TMEventResourceRangeAddEffectCreator creator)
        {
            base.Initialize(creator);

            Min = Mathf.Max(creator.Max);
            Max = Mathf.Min(creator.Min);

            int absMin = Mathf.Abs(Min);
            int absMax = Mathf.Abs(Max);
            int min = Mathf.Min(absMin, absMax);
            int max = Mathf.Max(absMax, absMin);

            _effectDescription.Arguments = new object[]
            {
                new
                {
                    Kind = Kind.ToString(),
                    Min = min,
                    Positive = Min >= 0 || Max >= 0, 
                    Max = max,
                }
            };
        }

        public override void ApplyEffect()
        {
            TMPlayerManager.Instance.AddResource(Kind, Random.Range(Min, Max + 1));
        }
    }
}