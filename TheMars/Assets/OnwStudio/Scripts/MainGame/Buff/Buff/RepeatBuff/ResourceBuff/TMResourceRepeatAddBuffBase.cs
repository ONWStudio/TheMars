using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;

namespace TM.Buff
{
    [System.Serializable]
    public abstract class TMResourceRepeatAddBuffBase : TMRepeatBuff, ITMInitializeBuff<TMResourceRepeatAddBuffBaseTrigger>
    {
        [field: SerializeField] public TMResourceKind Kind { get; set; }

        public void Initialize(TMResourceRepeatAddBuffBaseTrigger creator)
        {
            base.Initialize(creator);
            Kind = creator.ResourceKind;
        }
    }
}
