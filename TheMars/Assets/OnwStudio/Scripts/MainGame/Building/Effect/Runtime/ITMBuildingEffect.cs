using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Building.Effect
{
    public interface ITMBuildingEffect
    {
        void ApplyEffectLevelOne(TMBuilding owner);
        void ApplyEffectLevelTwo(TMBuilding owner);
        void ApplyEffectLevelThree(TMBuilding owner);
        void DisableEffect(TMBuilding owner);
    }
}

