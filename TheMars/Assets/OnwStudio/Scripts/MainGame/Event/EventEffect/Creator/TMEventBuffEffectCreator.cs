using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("���� ȿ��")]
    public class TMEventBuffEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeReference, SerializeReferenceDropdown, DisplayAs("����")] public ITMBuffTrigger BuffTrigger { get; private set; }

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.EventEffectGenerator.CreateEffect<TMEventBuffEffect, TMEventBuffEffectCreator>(this);
        }
    }
}
