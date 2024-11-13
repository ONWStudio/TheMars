using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("·£´ý °Ç¼³ ÀÚ¿ø È¹µæ ¹öÇÁ")]
    public class TMConstructionRandAddBuffTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField, DisplayAs("È¹µæ·®")] public int ResourceAdd { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMConstructionRandAddBuff, TMConstructionRandAddBuffTrigger>(this);
        }
    }
}
