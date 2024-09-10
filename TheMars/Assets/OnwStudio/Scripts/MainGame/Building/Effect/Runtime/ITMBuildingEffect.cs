using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.ServiceLocator;
using TM.Building.Effect.Creator;
using TM.Manager;

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

