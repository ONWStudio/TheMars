using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Extensions;
using TM.Event.Effect;
using TM.Event.Effect.Creator;
using TM.Grid;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Building.Effect
{
    [System.Serializable]
    public class TMEventRandBuildingGradeModifyEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventRandBuildingGradeModifyEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; } = new("TM_Event_Effect", "Rand_Building_Grade_Modify_Effect");
        [field: SerializeField, ReadOnly] public int TargetBuildingCount { get; set; }
        [field: SerializeField, ReadOnly] public int GradeAdd { get; set; }
        
        public void Initialize(TMEventRandBuildingGradeModifyEffectCreator creator)
        {
            TargetBuildingCount = creator.TargetBuildingCount;
            GradeAdd = creator.GradeAdd;
            EffectDescription.Arguments = new object[]
            {
                new
                {
                    TargetBuildingCount,
                    GradeAdd = Mathf.Abs(GradeAdd),
                    Positive = GradeAdd >= 0
                }
            };
        }
        
        public void ApplyEffect()
        {
            TMGridManager
                .Instance
                .Buildings
                .Values
                .OrderBy(_ => Random.value)
                .Take(TargetBuildingCount)
                .ForEach(building => building.Grade.Value += GradeAdd);
        }
    }
}
