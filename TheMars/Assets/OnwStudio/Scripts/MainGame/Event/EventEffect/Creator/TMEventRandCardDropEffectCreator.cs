using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    public enum TMCardKindForWhere
    {
        [InspectorName("�Ǽ�")] CONSTRUCTION,
        [InspectorName("ȿ��")] EFFECT,
        [InspectorName("���")] ALL
    }

    [System.Serializable, SerializeReferenceDropdownName("Ư�� ī�� ������ ����")]
    public class TMEventRandCardDropEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("ī�� ����")] public TMCardKindForWhere Kind { get; private set; } = TMCardKindForWhere.ALL;
        [field: SerializeField, DisplayAs("���� ī�� ����")] public int DropCount { get; private set; } = 0;

        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.EventEffectGenerator.CreateEffect<TMEventRandCardDropEffect, TMEventRandCardDropEffectCreator>(this);
        }
    }
}
