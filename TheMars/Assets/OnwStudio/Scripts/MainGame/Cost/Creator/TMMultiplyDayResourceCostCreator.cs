using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Manager;

namespace TM.Cost.Creator
{
    [SerializeField, SerializeReferenceDropdownName("������ ��Ƭ �Ҹ� (n������ ���)")]
    public class TMMultiplyDayResourceCostCreator : ITMCostCreator
    {
        [field: SerializeField, DisplayAs("�ڽ�Ʈ ����")] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("�⺻ ���η�")] public int Cost { get; private set; } = 10;

        public ITMCost CreateCost()
        {
            return ITMCostCreator.CreateEffect<TMMultiplyDayResourceCost, TMMultiplyDayResourceCostCreator>(this);
        }
    }
}