using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("마르스 리튬 납부 이벤트 납부량 추가 버프")]
    public class TMMarsLithiumEventModifyBuffTrigger : ITMBuffTrigger
    {
        [field: SerializeField, DisplayAs("종료 트리거 카운트")] public int EndTriggerCount { get; private set; }
        [field: SerializeField, DisplayAs("추가 납부량")] public int AddMarsLithium { get; private set; }
        [field: SerializeField, DisplayAs("영구 지속 여부")] public bool IsTemporay { get; private set; } = false;

        public TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMMarsLihitumEventModifyBuff, TMMarsLithiumEventModifyBuffTrigger>(this);
        }
    }
}
