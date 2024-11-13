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
        [SerializeField] private LocalizedString _effectDescription = new();

        public override LocalizedString EffectDescription => _effectDescription;

        [field: SerializeField] public int Min {get; set; }
        [field: SerializeField] public int Max { get; set; }

        public override void ApplyEffect()
        {
            TMPlayerManager.Instance.AddResource(Kind, Random.Range(Min, Max + 1));
        }

        public void Initialize(TMEventResourceRangeAddEffectCreator creator)
        {
            base.Initialize(creator);

            Min = Mathf.Max(Max);
            Max = Mathf.Min(Min);
        }
    }
}