using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("ÀÚ¿ø È¹µæ È¿°ú (¹üÀ§ ÁöÁ¤)")]
    public class TMResourceRepeatRangeAddBuffTrigger : TMResourceRepeatAddBuffBaseTrigger
    {
        [field: SerializeField, DisplayAs("ÃÖ¼Ò È¹µæ·®")] public int Min { get; private set; } = 0;
        [field: SerializeField, DisplayAs("ÃÖ´ë È¹µæ·®")] public int Max { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMResourceRepeatRangeAddBuff, TMResourceRepeatRangeAddBuffTrigger>(this);
        }
    }
}
