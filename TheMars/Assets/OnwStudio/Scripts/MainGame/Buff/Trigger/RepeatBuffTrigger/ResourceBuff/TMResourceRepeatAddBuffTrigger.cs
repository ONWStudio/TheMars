using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;

namespace TM.Buff
{
    [System.Serializable, SerializeReferenceDropdownName("자원 획득 버프")]
    public class TMResourceRepeatAddBuffTrigger : TMResourceRepeatAddBuffBaseTrigger
    {
        [field: SerializeField, DisplayAs("획득량")] public int Resource { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMResourceRepeatAddBuff, TMResourceRepeatAddBuffTrigger>(this);
        }
    }
}
