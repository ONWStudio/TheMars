using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Buff;
using TM.Buff.Trigger;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("랜덤한 건물 가동 중지 버프")]
    public class TMBuildingStopBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField, DisplayAs("가동중지할 건물 개수"), OnwMin(0)] public int TargetBuildingCount { get; private set; } = 0;
        
        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMBuildingStopBuff, TMBuildingStopBuffTrigger>(this);
        }
    }
}
