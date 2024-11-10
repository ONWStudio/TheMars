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
        [field: SerializeField, DisplayAs("반복 일수 간격")] public int RepeatDay { get; private set; } = 1;
        [field: SerializeField, DisplayAs("지속 일수")] public int LimitDay { get; private set; } = 1;
        [field: SerializeField, DisplayAs("영구 지속 여부")] public bool IsTemporary { get; private set; } = false;

        public abstract TMBuffBase CreateBuff();
    }
}
