using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Cost.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("자원 소모")]
    public class TMResourceCostCreator : ITMResourceCostCreator
    {
        [field: SerializeField, DisplayAs("자원 종류")] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("자원 소모량"), OnwMin(0)] public int Cost { get; private set; } = 0;

        public ITMCost CreateCost()
        {
            return ITMCostCreator.CreateEffect<TMResourceCost, TMResourceCostCreator>(this); 
        }
    }
}
