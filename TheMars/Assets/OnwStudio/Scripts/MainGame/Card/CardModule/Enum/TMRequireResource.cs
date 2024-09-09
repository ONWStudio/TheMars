using UnityEngine;
namespace TMCard
{
    public enum TMRequiredResource
    {
        [InspectorName("마르스 리튬")] MARS_LITHIUM = 1 << 1,
        [InspectorName("크레딧")] CREDIT = 1 << 2
    }
}
