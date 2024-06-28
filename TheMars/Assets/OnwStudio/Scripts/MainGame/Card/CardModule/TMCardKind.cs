using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    /// <summary>
    /// .. 발동 효과와 필요 자원에 따른 분류
    /// </summary>
    public enum TM_CARD_KIND : byte
    {
        /// <summary> .. 건설 </summary>
        [InspectorName("건설")] CONSTRUCTION,
        /// <summary> .. 노동 </summary>
        [InspectorName("일반")] NORMAL,
    }
}