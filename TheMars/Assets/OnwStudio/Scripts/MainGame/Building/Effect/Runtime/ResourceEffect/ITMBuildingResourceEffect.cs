using System.Collections;
using System.Collections.Generic;
using Onw.Interface;
using TM.Class;
using UnityEngine;

namespace TM.Building.Effect
{
    public interface ITMBuildingResourceEffect : ITMBuildingEffect
    {
        IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources { get; }
        float RepeatSeconds { get; set; }
        
        void AddResource(TMResourceKind resourceKind, int additionalAmount);
    }
}
