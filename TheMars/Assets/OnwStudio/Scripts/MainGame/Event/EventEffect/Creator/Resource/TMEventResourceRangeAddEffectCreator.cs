using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("ÀÚ¿ø È¹µæ È¿°ú (¹üÀ§)")]
    public class TMEventResourceRangeAddEffectCreator : TMEventResourceAddEffectBaseCreator
    {
        [field: SerializeField, DisplayAs("ÃÖ¼Ò È¹µæ·®")] public int Min {get; private set; } = 0;
        [field: SerializeField, DisplayAs("ÃÖ´ë È¹µæ·®")] public int Max { get; private set; } = 0;

        public override ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventResourceRangeAddEffect, TMEventResourceRangeAddEffectCreator>(this);
        }
    }
}
