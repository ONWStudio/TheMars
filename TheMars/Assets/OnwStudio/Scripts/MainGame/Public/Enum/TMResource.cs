using UnityEngine;

namespace TM
{
    public enum TMCostKind : byte
    {
        [InspectorName("마르스 리튬")] MARS_LITHIUM,
        [InspectorName("크레딧")] CREDIT,
        [InspectorName("강철")] STEEL,
        [InspectorName("식물")] PLANTS,
        [InspectorName("점토")] CLAY,
        [InspectorName("전기")] ELECTRICITY
    }

    public enum TMMainCost : byte
    {
        [InspectorName("크레딧")] CREDIT,
        [InspectorName("전기")] ELECTRICITY
    }

    public enum TMResourceKind : byte
    {
        [InspectorName("마르스 리튬")] MARS_LITHIUM,
        [InspectorName("크레딧")] CREDIT,
        [InspectorName("인구")] POPULATION,       
        [InspectorName("강철")] STEEL,
        [InspectorName("식물")] PLANTS,   
        [InspectorName("점토")] CLAY,
        [InspectorName("전기")] ELECTRICITY,
        [InspectorName("만족도")] SATISFACTION
    }

    public enum TMSubCost : byte
    {
        [InspectorName("마르스 리튬")] MARS_LITHIUM,
        [InspectorName("강철")] STEEL,
        [InspectorName("식물")] PLANTS,
        [InspectorName("점토")] CLAY
    }
}