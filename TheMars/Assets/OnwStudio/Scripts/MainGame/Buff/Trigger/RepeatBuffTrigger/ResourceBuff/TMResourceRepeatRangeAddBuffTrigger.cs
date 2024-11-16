using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("자원 획득 버프 (랜덤 범위)")]
    public class TMResourceRepeatRangeAddBuffTrigger : TMResourceRepeatAddBuffBaseTrigger
    {
        [field: SerializeField, DisplayAs("최소")] public int Min { get; private set; } = 0;
        [field: SerializeField, DisplayAs("최대")] public int Max { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMResourceRepeatRangeAddBuff, TMResourceRepeatRangeAddBuffTrigger>(this);
        }
    }
}
