using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("등장 카드 수 수정 버프")]
    public sealed class TMCollectCardModifyBuffTrigger : TMDelayBuffTrigger
    {
        [field: SerializeField, DisplayAs("등장하는 추가 카드 개수")] public int CollectCardCountAdd { get; private set; } = 0;

        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMCollectCardModifyBuff, TMCollectCardModifyBuffTrigger>(this);
        }
    }
}
