using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TM.Buff.Trigger;

namespace TM.Buff
{
    [System.Serializable, SerializeReferenceDropdownName("자원 획득 버프")]
    public class TMResourceRepeatAddBuffTrigger : TMResourceRepeatAddBuffBaseTrigger
    {
        [field: SerializeField, DisplayAs("획득량")] public int Resource { get; private set; }

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMResourceRepeatAddBuff, TMResourceRepeatAddBuffTrigger>(this);
        }

        public TMResourceRepeatAddBuffTrigger() {}

        public TMResourceRepeatAddBuffTrigger(int repeatDay, int limitDay, bool isTemporary, TMResourceKind kind, int resource) : base(repeatDay, limitDay, isTemporary, kind)
        {
            Resource = resource;
        }
    }
}
