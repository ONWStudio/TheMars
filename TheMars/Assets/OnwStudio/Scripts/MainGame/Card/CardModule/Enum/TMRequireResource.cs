using UnityEngine;
namespace TMCard
{
    public enum TMCostKind
    {
        [InspectorName("마르스 리튬")] MARS_LITHIUM = 1 << 1,
        [InspectorName("크레딧")] CREDIT = 1 << 2,
        [InspectorName("강철")] STEEL = 1 << 3,
        [InspectorName("식물")] PLANTS = 1 << 4,
        [InspectorName("점토")] CLAY = 1 << 5,
        [InspectorName("전기")] ELECTRICITY = 1 << 6,
        [InspectorName("인구")] POPULATION = 1 << 7
    }
}
