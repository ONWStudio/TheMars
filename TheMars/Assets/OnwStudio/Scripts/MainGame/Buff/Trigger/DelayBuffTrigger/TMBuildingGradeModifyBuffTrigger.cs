using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Building;
using UnityEngine;

namespace TM.Buff.Trigger
{
    public enum TMBuildingKindForWhere : byte
    {
        [InspectorName("개척형"), NonSerialized] PIONEER = 0,
        [InspectorName("자원형")] RESOURCEFUL,
        [InspectorName("개발형")] BUILDER,
        [InspectorName("외부형")] EXTERNAL,
        [InspectorName("모두")] ALL
    }
    
    [System.Serializable, SerializeReferenceDropdownName("건물 등급 버프")]
    public class TMBuildingGradeModifyBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField, DisplayAs("건물 종류")] public TMBuildingKindForWhere Kind { get; private set; } = TMBuildingKindForWhere.RESOURCEFUL;
        [field: SerializeField, DisplayAs("건물 개수")] public int TargetBuildingCount { get; private set; } = 0;
        [field: SerializeField, DisplayAs("등급 상승 수치")] public int GradeUpValue { get; private set; } = 0;
        
        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMBuildingGradeModifyBuff, TMBuildingGradeModifyBuffTrigger>(this);
        }
    }
}
