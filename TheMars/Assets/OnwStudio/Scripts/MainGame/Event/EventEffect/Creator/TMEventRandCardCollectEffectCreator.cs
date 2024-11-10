using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("ī�� ������ ȹ��")]
    public class TMEventRandCardCollectEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("ī�� ����")] public TMCardKindForWhere Kind { get; private set; } = TMCardKindForWhere.ALL;
        [field: SerializeField, DisplayAs("ī�� ȹ�淮")] public int CollectCount { get; private set; } = 1;

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.EventEffectGenerator.CreateEffect<TMEventRandCardCollectEffect, TMEventRandCardCollectEffectCreator>(this);
        }
    }
}