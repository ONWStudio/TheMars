using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("카드 선택 코스트 수정 버프")]
    public class TMCardCostModifyBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField] public TMCardKindForWhere CardKind { get; private set; } = TMCardKindForWhere.EFFECT;
        [field: SerializeField] public TMResourceKind CostKind { get; private set; } = TMResourceKind.MARS_LITHIUM;
        [field: SerializeField] public int AdditionalCost { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMCardCostModifyBuff, TMCardCostModifyBuffTrigger>(this);
        }
    }
}
