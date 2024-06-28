using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    public enum TM_CARD_GROUP : byte
    {
        [InspectorName("공통")] COMMON,
        [InspectorName("국가")] NATION,
        [InspectorName("기업")] CORPORATION
    }
}
