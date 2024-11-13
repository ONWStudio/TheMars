using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("�ڿ� ȹ�� ȿ�� (����)")]
    public class TMEventResourceRangeAddEffectCreator : TMEventResourceAddEffectBaseCreator
    {
        [field: SerializeField, DisplayAs("�ּ� ȹ�淮")] public int Min {get; private set; } = 0;
        [field: SerializeField, DisplayAs("�ִ� ȹ�淮")] public int Max { get; private set; } = 0;

        public override ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventResourceRangeAddEffect, TMEventResourceRangeAddEffectCreator>(this);
        }
    }
}
