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
    public sealed class TMResourceRepeatAddBuff : TMResourceRepeatAddBuffBase, ITMInitializeBuff<TMResourceRepeatAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference = new("Sprites/Smile_Buff_Positive");
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Resource_Repeat_Add_Buff");

        public override Color IconBackgroundColor => Color.blue;
        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField, ReadOnly] public int Resource { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMResourceRepeatAddBuffTrigger trigger)
        {
            base.Initialize(trigger);
            Resource = trigger.Resource;
            _description.Arguments = new object[]
            {
                new
                {
                    RepeatDay,
                    LimitDay,
                    IsTemporary,
                    Kind,
                    Resource = Mathf.Abs(Resource),
                    Positive = Resource > 0
                }
            };
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMPlayerManager.Instance.AddResource(Kind, Resource);
        }
    }
}
