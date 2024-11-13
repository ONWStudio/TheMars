using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable]
    public abstract class TMResourceRepeatAddBuffBaseTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField, DisplayAs("�ڿ� ����")] public TMResourceKind ResourceKind { get; private set; } = TMResourceKind.CREDIT;
    }
}
