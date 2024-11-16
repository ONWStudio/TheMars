using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("자원 획득 효과")]
    public class TMEventResourceAddEffectCreator : TMEventResourceAddEffectBaseCreator
    {
        [field: SerializeField, DisplayAs("자원 획득량")] public int Resource { get; private set; } = 0;
        
        public override ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventResourceAddEffect, TMEventResourceAddEffectCreator>(this);
        }
    }
}
