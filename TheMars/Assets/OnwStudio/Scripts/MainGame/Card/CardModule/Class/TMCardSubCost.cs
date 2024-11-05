using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TM.Card
{
    [System.Serializable]
    public sealed class TMCardSubCost
    {
        [field: SerializeField, DisplayAs("소모 재화 종류"), Tooltip("소모할 재화")]
        public TMSubCost CostKind { get; private set; }

        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화량")]
        public int Cost { get; private set; }
    }

    [System.Serializable]
    public sealed class TMCardMainCost
    {
        [field: SerializeField, DisplayAs("소모 재화 종류"), Tooltip("소모할 재화")]
        public TMMainCost CostKind { get; private set; }
        
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화량")]
        public int Cost { get; private set; }
    }
}