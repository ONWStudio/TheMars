using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using TM.Building.Effect;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [System.Serializable]
    public class TMConstructionRandAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionRandAddBuffTrigger>
    {
        [field: SerializeField] protected override AssetReferenceSprite IconReference { get; set; }
        [field: SerializeField] public int ResourceAdd { get; set; }

        public void Initialize(TMConstructionRandAddBuffTrigger creator)
        {
            base.Initialize(creator);

            ResourceAdd = creator.ResourceAdd; 
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMResourceKind kind = Random.Range(0, 3) switch
            {
                0 => TMResourceKind.STEEL,
                1 => TMResourceKind.PLANTS,
                _ => TMResourceKind.CLAY
            };

            TMPlayerManager.Instance.AddResource(kind, ResourceAdd);
        }
    }
}
