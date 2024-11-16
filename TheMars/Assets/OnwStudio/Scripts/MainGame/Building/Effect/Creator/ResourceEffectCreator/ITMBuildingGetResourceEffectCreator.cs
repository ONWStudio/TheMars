using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    public interface ITMBuildingGetResourceEffectCreator : ITMBuildingEffectCreator
    {
        TMResourceKind Kind { get; }
        int AdditionalResource { get; }
    }
}

