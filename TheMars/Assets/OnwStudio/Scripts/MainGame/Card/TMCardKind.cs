using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 발동 효과와 필요 자원에 따른 분류
/// </summary>
public enum TM_CARD_KIND : byte
{
    /// <summary> .. 건설 </summary>
    [InspectorName("건설")] CONSTRUCTION,
    /// <summary> .. 노동 </summary>
    [InspectorName("노동")] LABOR,
    /// <summary> .. 군사 </summary>
    [InspectorName("군사")] MILITARY,
    /// <summary> .. 탐험 </summary>
    [InspectorName("탐험")] EXPLORE,
    /// <summary> .. 정책 </summary>
    [InspectorName("정책")] POLICY
}
