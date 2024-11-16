using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("������ ��Ƭ ���� �̺�Ʈ ���η� �߰� ����")]
    public class TMMarsLithiumEventModifyBuffTrigger : ITMBuffTrigger
    {
        [field: SerializeField, DisplayAs("���� Ʈ���� ī��Ʈ")] public int EndTriggerCount { get; private set; }
        [field: SerializeField, DisplayAs("�߰� ���η�")] public int AddMarsLithium { get; private set; }
        [field: SerializeField, DisplayAs("���� ���� ����")] public bool IsTemporay { get; private set; } = false;

        public TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMMarsLihitumEventModifyBuff, TMMarsLithiumEventModifyBuffTrigger>(this);
        }
    }
}
