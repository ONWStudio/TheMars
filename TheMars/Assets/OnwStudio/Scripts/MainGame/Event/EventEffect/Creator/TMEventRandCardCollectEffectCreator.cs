using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("Ä«µå ¹«ÀÛÀ§ È¹µæ")]
    public class TMEventRandCardCollectEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("Ä«µå Á¾·ù")] public TMCardKindForWhere Kind { get; private set; } = TMCardKindForWhere.ALL;
        [field: SerializeField, DisplayAs("Ä«µå È¹µæ·®")] public int CollectCount { get; private set; } = 1;

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.EventEffectGenerator.CreateEffect<TMEventRandCardCollectEffect, TMEventRandCardCollectEffectCreator>(this);
        }
    }
}