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
    public sealed class TMResourceRepeatAddBuff : TMResourceRepeatAddBuffBase, ITMInitializeBuff<TMResourceRepeatAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Resource_Repeat_Add_Buff");

        public override Color IconBackgroundColor => Resource >= 0 ?
            ColorUtility.TryParseHtmlString("#2C138E", out Color blue) ? blue : Color.blue :
            ColorUtility.TryParseHtmlString("#8E3214", out Color red) ? red : Color.red;
        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField, ReadOnly] public int Resource { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMResourceRepeatAddBuffTrigger trigger)
        {
            base.Initialize(trigger);
            Resource = trigger.Resource;
            bool positive = Resource >= 0;
            _description.Arguments = new object[]
            {
                new
                {
                    RepeatDay,
                    LimitDay,
                    IsTemporary,
                    Kind,
                    Resource = Mathf.Abs(Resource),
                    Positive = positive
                }
            };

            string reference = Kind switch
            {
                TMResourceKind.POPULATION => positive ? "Population-plus" : "Popluation-minus",
                TMResourceKind.STEEL or TMResourceKind.PLANT or TMResourceKind.CLAY => positive ? "Construction-plus" : "Construction-minus",
                TMResourceKind.SATISFACTION => positive ? "Satisfaction_Icon" : "Satisfaction-minus",
                _ => positive ? "Cost-plus" : "Cost-minus",
            };

            _iconReference = new(reference);
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMPlayerManager.Instance.AddResource(Kind, Resource);
        }
    }
}
