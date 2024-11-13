using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable]
    public abstract class TMDelayBuffTrigger : ITMBuffTrigger
    {
        [field: SerializeField, DisplayAs("지연 일수"), OnwMin(0)] public int DelayDayCount { get; private set; } = 1;
        [field: SerializeField, DisplayAs("영구 지속 여부")] public bool IsTemporary { get; private set; }
        
        public abstract TMBuffBase CreateBuff();
    }
}
