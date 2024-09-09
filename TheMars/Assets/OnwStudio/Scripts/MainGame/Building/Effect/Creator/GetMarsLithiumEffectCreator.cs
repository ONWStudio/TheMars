using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    using static ITMBuildingEffectCreator;
    
    public abstract class GetResourceEffectCreator : ITMBuildingEffectCreator
    {
        [field: SerializeField, Min(1), DisplayAs("Level 1 반복 시간")] public float LevelOneRepeatSeconds { get; private set; }
        [field: SerializeField, Min(1), DisplayAs(("Level 1 재화"))] public int LevelOneResource { get; private set; }
        
        [field: SerializeField, Min(1), DisplayAs("Level 2 반복 시간")] public float LevelTwoRepeatSeconds { get; private set; }
        [field: SerializeField, Min(1), DisplayAs(("Level 2 재화"))] public int LevelTwoResource { get; private set; }     
        
        [field: SerializeField, Min(1), DisplayAs("Level 3 반복 시간")] public float LevelThreeRepeatSeconds { get; private set; }
        [field: SerializeField, Min(1), DisplayAs(("Level 3 재화"))] public int LevelThreeResource { get; private set; }
        public abstract ITMBuildingEffect CreateEffect();
    }
    
    public class GetMarsLithiumEffectCreator : GetResourceEffectCreator
    {
        public override ITMBuildingEffect CreateEffect()
        {
            return BuildingEffectGenerator.CreateEffect<GetMarsLithiumEffect, GetMarsLithiumEffectCreator>(this);
        }
    }
}

