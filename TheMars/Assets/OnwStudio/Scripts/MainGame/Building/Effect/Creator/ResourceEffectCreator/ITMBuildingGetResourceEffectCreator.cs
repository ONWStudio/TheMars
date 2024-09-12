using System.Collections;
using System.Collections.Generic;
using TM.Class;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    public interface ITMBuildingGetResourceEffectCreator : ITMBuildingEffectCreator
    {
        public IReadOnlyList<TMResourceData> Resources { get; }
    }
}

