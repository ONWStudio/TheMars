using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMConstructionOrMarsLithiumAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionOrMarsLithiumAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Construction_Or_MarsLithium_Add_Buff");
        [field: SerializeField] public int ResourceAdd { get; set; }
        public override AssetReferenceSprite IconReference => _iconReference;

        public override LocalizedString Description => _description;

        public void Initialize(TMConstructionOrMarsLithiumAddBuffTrigger creator)
        {
            base.Initialize(creator);

            ResourceAdd = creator.ResourceAdd;

            _description.Arguments = new object[]
            {
                new
                {
                    RepeatDay,
                    LimitDay,
                    IsTemporary,
                    Resource = Mathf.Abs(ResourceAdd),
                    Positive = ResourceAdd >= 0
                }
            };
        }

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
    }
}
