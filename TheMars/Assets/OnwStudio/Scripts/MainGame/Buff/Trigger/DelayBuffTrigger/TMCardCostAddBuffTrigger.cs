using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("카드 코스트 수정 버프")]
    public class TMCardCostAddBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField, DisplayAs("카드 종류")] public TMCardKindForWhere Kind { get; private set; } = TMCardKindForWhere.EFFECT;
        [field: SerializeField, DisplayAs("추가 코스트")] public int AdditionalCost { get; private set; } = 0;
        
        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMCardCostAddBuff, TMCardCostAddBuffTrigger>(this);
        }
    }
}
