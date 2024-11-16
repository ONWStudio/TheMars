using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using TM.Building.Effect;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMConstructionRandAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionRandAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Construction_Rand_Add_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;
        [field: SerializeField] public int ResourceAdd { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMConstructionRandAddBuffTrigger creator)
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
                    Positive = ResourceAdd > 0
                }
            };
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
