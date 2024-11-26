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
        [field: SerializeField, DisplayAs("효과 발동 텀")] public int RepeatDay { get; private set; } = 1;
        [field: SerializeField, DisplayAs("효과 제한 일수")] public int LimitDay { get; private set; } = 1;
        [field: SerializeField, DisplayAs("영구 지속 여부")] public bool IsTemporary { get; private set; } = false;

        public abstract TMBuffBase CreateBuff();

        protected TMRepeatBuffTrigger() {}
        protected TMRepeatBuffTrigger(int repeatDay, int limitDay, bool isTemporary)
        {
            RepeatDay = repeatDay;
            LimitDay = limitDay;
            IsTemporary = isTemporary;
        }
    }
}
