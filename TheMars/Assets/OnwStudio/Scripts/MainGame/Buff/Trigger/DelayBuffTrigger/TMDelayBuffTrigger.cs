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
        
        public abstract TMBuffBase CreateBuff();
    }
}
