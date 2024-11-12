using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("\"마르스 리튬\" 납부 이벤트 납부량 수정 효과")]
    public class TMEventMarsLithiumEventModifyEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("추가 납부량")] public int MarsLithiumAdd { get; private set; } = 0;
        
        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventMarsLithiumEventModifyEffect, TMEventMarsLithiumEventModifyEffectCreator>(this);
        }
    }
}
