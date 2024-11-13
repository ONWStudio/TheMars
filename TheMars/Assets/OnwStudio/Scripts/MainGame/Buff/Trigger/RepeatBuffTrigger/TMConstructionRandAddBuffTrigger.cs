using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("건설 자원 랜덤 획득")]
    public class TMConstructionRandAddBuffTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField, DisplayAs("획득량")] public int ResourceAdd { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMConstructionRandAddBuff, TMConstructionRandAddBuffTrigger>(this);
        }
    }
}
