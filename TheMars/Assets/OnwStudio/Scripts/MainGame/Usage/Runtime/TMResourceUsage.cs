using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Usage.Creator;

namespace TM.Usage
{
    [System.Serializable]
    public class TMResourceUsage : ITMUsage, ITMInitializeUsage<TMResourceUsageCreator>
    {
        public bool CanProcessPayment => TMPlayerManager.Instance.GetResoucesByKind(Kind) >= ResourceUsage;

        [field: SerializeField, ReadOnly] public LocalizedString UsageLocalizedString { get; private set; } = new("TM_Cost", "Resource_Usage");
        [field: SerializeField, ReadOnly] public TMResourceKind Kind { get; set; }
        [field: SerializeField, ReadOnly] public int ResourceUsage { get; set; }

        public void Initialize(TMResourceUsageCreator creator)
        {
            Kind = creator.ResourceKind;
            ResourceUsage = creator.ResourceUsage;

            UsageLocalizedString.Arguments = new object[]
            {
                new
                {
                    Kind,
                    Resource = ResourceUsage
                }
            };
        }

        public void ApplyUsage()
        {
            TMPlayerManager.Instance.AddResource(Kind, -ResourceUsage);
        }
    }
}

