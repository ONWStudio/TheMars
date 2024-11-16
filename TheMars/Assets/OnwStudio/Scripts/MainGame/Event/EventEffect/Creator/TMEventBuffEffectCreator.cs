using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("버프 효과")]
    public class TMEventBuffEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeReference, SerializeReferenceDropdown, DisplayAs("버프")] public ITMBuffTrigger BuffTrigger { get; private set; }

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventBuffEffect, TMEventBuffEffectCreator>(this);
        }
    }
}
