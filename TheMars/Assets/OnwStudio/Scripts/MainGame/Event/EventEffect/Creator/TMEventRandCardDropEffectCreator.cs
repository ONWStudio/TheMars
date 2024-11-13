using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("랜덤 카드 버리기 효과")]
    public class TMEventRandCardDropEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("카드 종류")] public TMCardKindForWhere Kind { get; private set; } = TMCardKindForWhere.ALL;
        [field: SerializeField, DisplayAs("버릴 카드 개수")] public int DropCount { get; private set; } = 0;

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventRandCardDropEffect, TMEventRandCardDropEffectCreator>(this);
        }
    }
}
