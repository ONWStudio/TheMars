using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using TM.Building.Effect.Creator;
using TM.Manager;
using UnityEngine.Localization;

namespace TM.Building.Effect
{
    public interface ITMBuildingEffect
    {
        LocalizedString LocalizedDescription { get; }

        void ApplyEffect(TMBuilding owner);
        void DisableEffect(TMBuilding owner);
    }
}

