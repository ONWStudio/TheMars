using System.Collections;
using System.Collections.Generic;
using Onw.Interface;
using UnityEngine;

namespace TM.Building.Effect
{
    public interface ITMBuildingResourceEffect : ITMBuildingEffect
    {
        TMResourceKind Kind { get; }
        int AdditionalResource { get; }
        float RepeatSeconds { get; set; }
        
        void AddResource(int additionalAmount);
    }
}
