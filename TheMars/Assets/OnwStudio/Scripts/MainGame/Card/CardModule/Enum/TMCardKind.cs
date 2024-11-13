using UnityEngine;
namespace TM
{
    public enum TMCardKind : byte
    {
        [InspectorName("건설")] CONSTRUCTION,
        [InspectorName("효과")] EFFECT
    }
    
    public enum TMCardKindForWhere
    {
        [InspectorName("건설")] CONSTRUCTION,
        [InspectorName("효과")] EFFECT,
        [InspectorName("모두")] ALL
    }
}