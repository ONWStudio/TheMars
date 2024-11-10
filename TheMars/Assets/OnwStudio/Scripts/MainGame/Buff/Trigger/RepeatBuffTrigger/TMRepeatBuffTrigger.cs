using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using TM.Buff;
using UnityEngine;
using Onw.Attribute;

namespace TM.Buff.Trigger
{ 
    [System.Serializable]
    public abstract class TMRepeatBuffTrigger : ITMBuffTrigger
    {
        [field: SerializeField, DisplayAs("�ݺ� �ϼ� ����")] public int RepeatDay { get; private set; } = 1;
        [field: SerializeField, DisplayAs("���� �ϼ�")] public int LimitDay { get; private set; } = 1;
        [field: SerializeField, DisplayAs("���� ���� ����")] public bool IsTemporary { get; private set; } = false;

        public abstract TMBuffBase CreateBuff();
    }
}
