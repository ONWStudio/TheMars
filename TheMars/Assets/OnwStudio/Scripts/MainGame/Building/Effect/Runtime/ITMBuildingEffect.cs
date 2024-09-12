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
        void ApplyEffect(TMBuilding owner);
        void DisableEffect(TMBuilding owner);
    }
}

