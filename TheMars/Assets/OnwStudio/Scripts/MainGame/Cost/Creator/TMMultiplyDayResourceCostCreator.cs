using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Manager;

namespace TM.Cost.Creator
{
    [SerializeField, SerializeReferenceDropdownName("n일차 배수 자원 납부")]
    public class TMMultiplyDayResourceCostCreator : ITMCostCreator
    {
        [field: SerializeField, DisplayAs("자원 종류")] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("납부량")] public int Cost { get; private set; } = 10;

        public ITMCost CreateCost()
        {
            return ITMCostCreator.CreateEffect<TMMultiplyDayResourceCost, TMMultiplyDayResourceCostCreator>(this);
        }
    }
}