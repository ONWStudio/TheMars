using System.Collections;
using System.Collections.Generic;
using TM.Building.Effect.Creator;
using UnityEngine;

namespace TM.Building.Effect
{
    public interface ITMBuildingInitializeEffect<in T> where T : ITMBuildingEffectCreator
    {
        void Initialize(T effectCreator);
    }
}

