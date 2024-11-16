using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Building.Effect;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable, SerializeReferenceDropdownName("랜덤 빌딩 등급 수정 효과(영구적)")]
    public sealed class TMEventRandBuildingGradeModifyEffectCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("등급업 시킬 건물 개수")] public int TargetBuildingCount { get; private set; } = 0;
        [field: SerializeField, DisplayAs("등급 상승 수치")] public int GradeAdd { get; private set; } = 0;
        
        public ITMEventEffect CreateEffect()
        {
            return ITMEventEffectCreator.CreateEffect<TMEventRandBuildingGradeModifyEffect, TMEventRandBuildingGradeModifyEffectCreator>(this);
        }
    }
}
