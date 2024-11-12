using System.Collections;
using System.Collections.Generic;
using TM.Building;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("건물 등급 버프")]
    public class TMBuildingGradeModifyBuffTrigger : TMDelayBuffTrigger
    {
        
        
        public override TMBuffBase CreateBuff()
        {
            TMBuildingData
            return ITMBuffTrigger.CreateBuff<TMBuildingGradeModifyBuff, TMBuildingGradeModifyBuffTrigger>(this);
        }
    }
}
