using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class Card : ScriptableObject
{
    /// <summary>
    /// .. 발동 효과와 필요 자원에 따른 분류
    /// </summary>
    public enum CARD_KIND
    {
        /// <summary> .. 건설 </summary>
        CONSTRUCTION,
        /// <summary> .. 노동 </summary>
        LABOR,
        /// <summary> .. 군사 </summary>
        MILITARY,
        /// <summary> .. 탐험 </summary>
        EXPLORE,
        /// <summary> .. 정책 </summary>
        POLICY
    }
}
