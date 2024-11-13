using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("���� �Ǽ� �ڿ� ȹ�� ����")]
    public class TMConstructionRandAddBuffTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField, DisplayAs("ȹ�淮")] public int ResourceAdd { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMConstructionRandAddBuff, TMConstructionRandAddBuffTrigger>(this);
        }
    }
}
