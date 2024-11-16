using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect.Creator;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("�̺�Ʈ Ȯ�� �߰� ����")]
    public class TMEventAdditionalProbabilityBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField, DisplayAs("�̺�Ʈ ����")] public TMEventKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("�߰� Ȯ��")] public int AddProbabiltiy { get; private set; }

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMEventAdditionalProbabilityBuff, TMEventAdditionalProbabilityBuffTrigger>(this);
        }
    }
}
