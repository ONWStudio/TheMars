using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("자원 획득 효과")]
    public class TMEventResourceAddEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("자원 종류")] public TMResourceKind ResourceKind { get; private set; } = TMResourceKind.CLAY;
        [field: SerializeField, DisplayAs("자원 획득량")] public int Resource { get; private set; } = 0;
        
        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.EventEffectGenerator.CreateEffect<TMEventResourceAddEffect, TMEventResourceAddEffectCreator>(this);
        }
    }
}
