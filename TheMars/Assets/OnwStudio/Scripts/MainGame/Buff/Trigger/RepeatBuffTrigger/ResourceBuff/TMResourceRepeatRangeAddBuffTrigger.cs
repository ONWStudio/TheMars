using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("�ڿ� ȹ�� ȿ�� (���� ����)")]
    public class TMResourceRepeatRangeAddBuffTrigger : TMResourceRepeatAddBuffBaseTrigger
    {
        [field: SerializeField, DisplayAs("�ּ� ȹ�淮")] public int Min { get; private set; } = 0;
        [field: SerializeField, DisplayAs("�ִ� ȹ�淮")] public int Max { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMResourceRepeatRangeAddBuff, TMResourceRepeatRangeAddBuffTrigger>(this);
        }
    }
}
