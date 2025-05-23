using UnityEngine;
namespace TM
{
    /// <summary>
    /// .. 발동 효과에 따른 카드 등급
    /// </summary>
    public enum TMCardGrade : byte
    {
        /// <summary> .. 일반 </summary>>
        [InspectorName("일반")] NORMAL,
        /// <summary> .. 고급 </summary>>
        [InspectorName("고급")] HIGH,
        /// <summary> .. 희귀 </summary>>
        [InspectorName("희귀")] RARE,
        /// <summary> .. 유일 </summary>>
        [InspectorName("유일")] ONLY
    }
}