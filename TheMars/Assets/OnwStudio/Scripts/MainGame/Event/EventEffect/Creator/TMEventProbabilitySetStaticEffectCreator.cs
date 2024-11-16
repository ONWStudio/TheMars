using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    public enum TMEventKind : byte
    {
        [InspectorName("재난")] CALAMITY,
        [InspectorName("긍정")] POSITIVE,
        [InspectorName("부정")] NEGATIVE
    }
    
    [System.Serializable, SerializeReferenceDropdownName("이벤트 확률 고정 효과")]
    public class TMEventProbabilitySetStaticEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("이벤트 종류")] public TMEventKind Kind { get; private set; } = TMEventKind.CALAMITY;
        [field: SerializeField, DisplayAs("고정 확률")] public int Probability { get; private set; }

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventProbabilitySetStaticEffect, TMEventProbabilitySetStaticEffectCreator>(this);
        }
    }
}
