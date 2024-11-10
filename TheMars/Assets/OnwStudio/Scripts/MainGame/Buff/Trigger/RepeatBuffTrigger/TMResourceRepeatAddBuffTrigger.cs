using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;

namespace TM.Buff
{
    [System.Serializable, SerializeReferenceDropdownName("ÀÚ¿ø È¹µæ ¹öÇÁ")]
    public class TMResourceRepeatAddBuffTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField, DisplayAs("È¹µæ ÇÒ ÀÚ¿ø")] public TMResourceKind ResourceKind { get; private set; } = TMResourceKind.CREDIT;
        [field: SerializeField, DisplayAs("È¹µæ ÇÒ ÀÚ¿ø·®")] public int Resource { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMResourceRepeatAddBuff, TMResourceRepeatAddBuffTrigger>(this);
        }
    }
}
