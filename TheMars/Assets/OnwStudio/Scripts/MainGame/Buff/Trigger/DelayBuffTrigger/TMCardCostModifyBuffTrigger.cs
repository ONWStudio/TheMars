using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("ī�� ���� �ڽ�Ʈ ���� ����")]
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
