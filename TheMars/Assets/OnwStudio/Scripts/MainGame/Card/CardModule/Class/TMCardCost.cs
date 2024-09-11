using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TM.Card
{
    [System.Serializable]
    public sealed class TMCardCost
    {
        [field: SerializeField, DisplayAs("소모 재화 종류"), Tooltip("소모할 재화")]
        public TMResourceKind ResourceKind { get; private set; }

        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화량")]
        public int Cost { get; private set; }
    }
}