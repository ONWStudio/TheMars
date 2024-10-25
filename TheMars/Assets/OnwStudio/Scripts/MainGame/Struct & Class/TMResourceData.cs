using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Class
{
    [System.Serializable]
    public class TMResourceData
    {
        [field: SerializeField, DisplayAs("획득 재화 종류"), Tooltip("획득 할 재화")]
        public TMResourceKind ResourceKind { get; private set; }

        [field: SerializeField, DisplayAs("획득 재화"), Tooltip("획득 재화량")]
        public int Resource { get; private set; }
    }

    [System.Serializable]
    public class TMResourceDataForRuntime
    {
        [field: SerializeField, ReadOnly]
        public TMResourceKind ResourceKind { get; private set; }

        [field: SerializeField, ReadOnly]
        public int Resource { get; private set; }

        [field: SerializeField, ReadOnly]
        public List<int> AdditionalResources { get; private set; } = new();

        public int AdditionalResource => AdditionalResources.Sum();
        public int FinalResource => Resource + AdditionalResource;
        
        public static implicit operator TMResourceDataForRuntime(in TMResourceData resourceData)
        {
            return new()
            {
                ResourceKind = resourceData.ResourceKind,
                Resource = resourceData.Resource
            };
        }
    }
}

