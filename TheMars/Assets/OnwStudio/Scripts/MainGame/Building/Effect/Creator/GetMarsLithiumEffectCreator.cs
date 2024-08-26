using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    using static ITMBuildingEffectCreator;
    
    public class GetMarsLithiumEffectCreator : ITMBuildingEffectCreator
    {
        // .. TODO : 구조 수정
        [field: SerializeField, Min(1), DisplayAs("Level 1 반복 시간")] public int LevelOneRepeatSeconds { get; private set; }
        [field: SerializeField, Min(1), DisplayAs(("Level 1 마르스 리튬"))] public int LevelOneMarsLithium { get; private set; }
        
        [field: SerializeField, Min(1), DisplayAs("Level 2 반복 시간")] public int LevelTwoRepeatSeconds { get; private set; }
        [field: SerializeField, Min(1), DisplayAs(("Level 2 마르스 리튬"))] public int LevelTwoMarsLithium { get; private set; }     
        
        [field: SerializeField, Min(1), DisplayAs("Level 3 반복 시간")] public int LevelThreeRepeatSeconds { get; private set; }
        [field: SerializeField, Min(1), DisplayAs(("Level 3 마르스 리튬"))] public int LevelThreeMarsLithium { get; private set; }
        
        public ITMBuildingEffect CreateEffect()
        {
            return BuildingEffectGenerator.CreateEffect<GetMarsLithiumEffect, GetMarsLithiumEffectCreator>(this);
        }
    }
}

