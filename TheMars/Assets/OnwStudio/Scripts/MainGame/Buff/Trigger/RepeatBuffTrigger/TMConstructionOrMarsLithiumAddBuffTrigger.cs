using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff.Trigger
{
    [System.Serializable, SerializeReferenceDropdownName("건설 자원 또는 마르스 리튬 획득 버프")]    
    public sealed class TMConstructionOrMarsLithiumAddBuffTrigger : TMRepeatBuffTrigger
    {
        [field: SerializeField, DisplayAs("획득량")] public int ResourceAdd { get; private set; } = 0;
        
        public override TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMConstructionOrMarsLithiumAddBuff, TMConstructionOrMarsLithiumAddBuffTrigger>(this);
        }
    }
}
