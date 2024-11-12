using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [System.Serializable]
    public class TMConstructionOrMarsLithiumAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionOrMarsLithiumAddBuffTrigger>
    {
        [field: SerializeField] public int ResourceAdd { get; set; }
        protected override AssetReferenceSprite IconReference { get; set; }
        
        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMResourceKind kind = Random.Range(0, 5) switch
            {
                0 => TMResourceKind.PLANTS,
                1 => TMResourceKind.CLAY,
                2 => TMResourceKind.STEEL,
                _ => TMResourceKind.MARS_LITHIUM,
            };
            
            TMPlayerManager.Instance.AddResource(kind, ResourceAdd);
        }
        
        public void Initialize(TMConstructionOrMarsLithiumAddBuffTrigger creator)
        {
            base.Initialize(creator);

            ResourceAdd = creator.ResourceAdd;
        }
    }
}
