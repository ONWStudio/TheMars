using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    using static ITMBuildingEffectCreator;
    
    public class GetMarsLithiumEffectCreator : GetResourceEffectCreator
    {
        public override ITMBuildingEffect CreateEffect()
        {
            return BuildingEffectGenerator.CreateEffect<GetMarsLithiumEffect, GetMarsLithiumEffectCreator>(this);
        }
    }
}

