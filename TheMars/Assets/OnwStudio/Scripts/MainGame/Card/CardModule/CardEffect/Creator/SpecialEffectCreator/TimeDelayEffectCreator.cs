using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("소요 (시간)"), Substitution("소요(시간)")]
    public class TimeDelayEffectCreator : ITMEffectCreator
    {
        [field: SerializeField, DisplayAs("딜레이 시간 (초)")] public float DelayTime { get; private set; } = 1f;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TimeDelayEffect, TimeDelayEffectCreator>(this);
        }
    }
}
