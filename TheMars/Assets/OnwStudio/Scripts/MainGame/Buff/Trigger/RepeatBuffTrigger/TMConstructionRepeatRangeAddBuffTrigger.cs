using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("�Ǽ� �ڿ� ȹ�� ���� (����)")]
    public class TMConstructionRepeatRangeAddBuffTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField] public int Min {get; private set;} = 0;
        [field: SerializeField] public int Max { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMConstructionRepeatRangeAddBuff, TMConstructionRepeatRangeAddBuffTrigger>(this);
        }
    }
}
