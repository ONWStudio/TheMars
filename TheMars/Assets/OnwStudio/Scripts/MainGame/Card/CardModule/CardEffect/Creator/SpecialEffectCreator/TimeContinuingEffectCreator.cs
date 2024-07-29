using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 지속 (시간)"), Substitution("지속(시간)")]
    public class TimeContinuingEffectCreator : ITMSpecialEffectCreator
    {
        [field: SerializeField, DisplayAs("지속 시간")] public float ContinuingTime { get; private set; } = 1f;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TimeContinuingEffect, TimeContinuingEffectCreator>(this);
        }
    }
}
