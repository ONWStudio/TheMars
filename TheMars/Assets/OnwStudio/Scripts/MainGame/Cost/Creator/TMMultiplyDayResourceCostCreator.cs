using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Manager;

namespace TM.Cost.Creator
{
    [SerializeField, SerializeReferenceDropdownName("마르스 리튬 소모 (n일차의 배수)")]
    public class TMMultiplyDayResourceCostCreator : ITMCostCreator
    {
        [field: SerializeField, DisplayAs("코스트 종류")] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("기본 납부량")] public int Cost { get; private set; } = 10;

        public ITMCost CreateCost()
        {
            return ITMCostCreator.CreateEffect<TMMultiplyDayResourceCost, TMMultiplyDayResourceCostCreator>(this);
        }
    }
}