using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("랜덤 카드 획득 효과")]
    public class TMEventRandCardCollectEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("카드 종류")] public TMCardKindForWhere Kind { get; private set; } = TMCardKindForWhere.ALL;
        [field: SerializeField, DisplayAs("카드 획득량")] public int CollectCount { get; private set; } = 1;

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventRandCardCollectEffect, TMEventRandCardCollectEffectCreator>(this);
        }
    }
}