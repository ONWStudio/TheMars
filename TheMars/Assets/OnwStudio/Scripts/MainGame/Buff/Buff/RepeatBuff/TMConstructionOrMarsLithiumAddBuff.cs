using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace TM.Buff
{
    [System.Serializable]
    public class TMConstructionOrMarsLithiumAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionOrMarsLithiumAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Construction_Or_MarsLithium_Add_Buff");
        [field: SerializeField] public int ResourceAdd { get; set; }

        public override Color IconBackgroundColor => ResourceAdd >= 0 ? 
            ColorUtility.TryParseHtmlString("#2C138E", out Color blue) ? blue : Color.blue :
            ColorUtility.TryParseHtmlString("#8E3214", out Color red) ? red : Color.red;

        public override AssetReferenceSprite IconReference => _iconReference;

        public override LocalizedString Description => _description;

        public void Initialize(TMConstructionOrMarsLithiumAddBuffTrigger creator)
        {
            base.Initialize(creator);

            ResourceAdd = creator.ResourceAdd;
            bool positive = ResourceAdd >= 0;

            _description.Arguments = new object[]
            {
                new
                {
                    RepeatDay,
                    LimitDay,
                    IsTemporary,
                    Resource = Mathf.Abs(ResourceAdd),
                    Positive = positive
                }
            };

            _iconReference = new(positive ? "Construction-plus" : "Construction-minus");
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMResourceKind kind = Random.Range(0, 5) switch
            {
                0 => TMResourceKind.PLANT,
                1 => TMResourceKind.CLAY,
                2 => TMResourceKind.STEEL,
                _ => TMResourceKind.MARS_LITHIUM,
            };
            
            TMPlayerManager.Instance.AddResource(kind, ResourceAdd);
        }
    }
}
