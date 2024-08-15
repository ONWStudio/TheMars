using UnityEngine;
namespace TMCard
{
    /// <summary>
    /// .. 발동 효과와 필요 자원에 따른 분류
    /// </summary>
    public enum TMCardKind : byte
    {
        /// <summary> .. 건설 </summary>
        [InspectorName("일반")] NONE = 1 << 0,
        /// <summary> .. 노동 </summary>
        [InspectorName("드론")] DRON = 1 << 1,
        [InspectorName("미생물")] MICROBE = 1 << 2,
        [InspectorName("모두")] ALL = NONE | DRON | MICROBE 
    }
}