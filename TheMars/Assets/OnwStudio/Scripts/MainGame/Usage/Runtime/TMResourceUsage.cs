using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Usage.Creator;
using UnityEngine.Localization.Components;


namespace TM.Usage
{
    public class TMResourceUsage : ITMUsage, ITMInitializeUsage<TMResourceUsageCreator>
    {
        public bool CanProcessPayment => TMPlayerManager.Instance.GetResoucesByKind(ResourceKind) >= ResourceUsage;

        [field: SerializeField, ReadOnly] public LocalizedString UsageLocalizedString { get; private set; } = null;
        [field: SerializeField, ReadOnly] public TMResourceKind ResourceKind { get; set; }
        [field: SerializeField, ReadOnly] public int ResourceUsage { get; set; }


        public void Initialize(TMResourceUsageCreator creator)
        {
            ResourceKind = creator.ResourceKind;
            ResourceUsage = creator.ResourceUsage;
            string tableEntryName = ResourceKind switch
            {
                TMResourceKind.MARS_LITHIUM => "MarsLithium",
                TMResourceKind.CREDIT => "Credit",
                TMResourceKind.POPULATION => "Population",
                TMResourceKind.STEEL => "Steel",
                TMResourceKind.PLANTS => "Plants",
                TMResourceKind.CLAY => "Clay",
                TMResourceKind.ELECTRICITY => "Electricity",
                _ => ""
            };


            UsageLocalizedString = new("TMUsage", tableEntryName)
            { 
                Arguments = new object[] { new Dictionary<string, object>() { { "ResourceUsage", ResourceUsage } } }
            };
        }

        public void ApplyUsage()
        {
            TMPlayerManager.Instance.AddResource(ResourceKind, -ResourceUsage);
        }
    }
}

