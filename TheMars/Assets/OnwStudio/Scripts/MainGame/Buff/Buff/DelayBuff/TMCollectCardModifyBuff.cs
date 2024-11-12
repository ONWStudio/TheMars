using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Buff.Trigger;
using TM.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [System.Serializable]
    public sealed class TMCollectCardModifyBuff : TMDelayBuff, ITMInitializeBuff<TMCollectCardModifyBuffTrigger>
    {
        [field: SerializeField, ReadOnly] protected override AssetReferenceSprite IconReference { get; set; }
        [field: SerializeField] public int CollectCardCountAdd { get; set; }

        protected override void OnApplyBuff()
        {
            TMCardNotifyIconSpawner.Instance.CardCreationCount += CollectCardCountAdd;
        }
        
        protected override void OnChangedDayByDelayCount(int day)
        {
            TMCardNotifyIconSpawner.Instance.CardCreationCount -= CollectCardCountAdd;
        }
        
        public void Initialize(TMCollectCardModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            CollectCardCountAdd = creator.CollectCardCountAdd;
        }
    }
}
