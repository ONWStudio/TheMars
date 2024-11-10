using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Usage.Creator
{
    [SerializeReferenceDropdownName("�ڿ� �Ҹ�")]
    public class TMResourceUsageCreator : ITMUsageCreator
    {
        [field: SerializeField, DisplayAs("�Ҹ� �ڿ�")] public TMResourceKind ResourceKind { get; private set; }
        [field: SerializeField, DisplayAs("�Ҹ�"), OnwMin(0)] public int ResourceUsage { get; private set; } = 0;

        public ITMUsage CreateUsage()
        {
            return ITMUsageCreator.UsageGenerator.CreateEffect<TMResourceUsage, TMResourceUsageCreator>(this); 
        }
    }
}
