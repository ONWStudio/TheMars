using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect;
using TM.Event.Effect.Creator;
using UnityEngine;

namespace TM.Event.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("게임 종료 효과")]
    public sealed class TMEventGameEndEffectCreator : ITMEventEffectCreator
    {
        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventGameEndEffect, TMEventGameEndEffectCreator>(this);
        }
    }
}
