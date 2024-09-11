using UnityEngine;

namespace TM
{
    public enum TMResourceKind
    {
        [InspectorName("마르스 리튬")] MARS_LITHIUM = 0,
        [InspectorName("크레딧")] CREDIT = 1,
        [InspectorName("인구")] POPULATION = 2,
        [InspectorName("강철")] STEEL = 3,
        [InspectorName("식물")] PLANTS = 4,
        [InspectorName("점토")] CLAY = 5,
        [InspectorName("전기")] ELECTRICITY = 6
    }
}
