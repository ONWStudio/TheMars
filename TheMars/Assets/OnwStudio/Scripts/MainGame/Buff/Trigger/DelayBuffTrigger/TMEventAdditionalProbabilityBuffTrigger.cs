using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect.Creator;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("이벤트 확률 추가 버프")]
    public class TMEventAdditionalProbabilityBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField, DisplayAs("이벤트 종류")] public TMEventKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("추가 확률")] public int AddProbabiltiy { get; private set; }

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMEventAdditionalProbabilityBuff, TMEventAdditionalProbabilityBuffTrigger>(this);
        }
    }
}
