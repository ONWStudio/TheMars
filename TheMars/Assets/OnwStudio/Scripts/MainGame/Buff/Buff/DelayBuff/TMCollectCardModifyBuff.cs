using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Buff.Trigger;
using TM.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public sealed class TMCollectCardModifyBuff : TMDelayBuff, ITMInitializeBuff<TMCollectCardModifyBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Collect_Card_Modify_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;
        [field: SerializeField] public int CollectCardCountAdd { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMCollectCardModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            CollectCardCountAdd = creator.CollectCardCountAdd;
            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    Count = Mathf.Abs(CollectCardCountAdd),
                    Positive = CollectCardCountAdd >= 0
                }
            };
        }

        protected override void OnApplyBuff()
        {
            TMCardNotifyIconSpawner.Instance.CardCreationCount += CollectCardCountAdd;
        }
        
        protected override void OnChangedDayByDelayCount(int day)
        {
            TMCardNotifyIconSpawner.Instance.CardCreationCount -= CollectCardCountAdd;
        }
    }
}
