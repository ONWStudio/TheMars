using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Usage.Creator
{
    [SerializeReferenceDropdownName("자원 소모")]
    public class TMResourceUsageCreator : ITMUsageCreator
    {
        [field: SerializeField, DisplayAs("자원 종류")] public TMResourceKind ResourceKind { get; private set; }
        [field: SerializeField, DisplayAs("자원 소모량"), OnwMin(0)] public int ResourceUsage { get; private set; } = 0;

        public ITMUsage CreateUsage()
        {
            return ITMUsageCreator.CreateEffect<TMResourceUsage, TMResourceUsageCreator>(this); 
        }
    }
}
