using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("자원 획득 효과 (랜덤 범위)")]
    public class TMEventResourceRangeAddEffectCreator : TMEventResourceAddEffectBaseCreator
    {
        [field: SerializeField, DisplayAs("최소")] public int Min {get; private set; } = 0;
        [field: SerializeField, DisplayAs("최대")] public int Max { get; private set; } = 0;

        public override ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventResourceRangeAddEffect, TMEventResourceRangeAddEffectCreator>(this);
        }
    }
}
