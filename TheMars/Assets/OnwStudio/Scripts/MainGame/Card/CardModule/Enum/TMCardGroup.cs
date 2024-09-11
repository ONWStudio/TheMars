using UnityEngine;
namespace TM
{
    public enum TMCardGroup : byte
    {
        [InspectorName("공통")] COMMON,
        [InspectorName("국가")] NATION,
        [InspectorName("기업")] CORPORATION
    }
}
