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
    public class TMResourceRepeatRangeAddBuff : TMResourceRepeatAddBuffBase, ITMInitializeBuff<TMResourceRepeatRangeAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Resource_Repeat_Range_Add_Buff"); 

        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField] public int Min { get; set; }
        [field: SerializeField] public int Max { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMResourceRepeatRangeAddBuffTrigger creator)
        {
            base.Initialize(creator);

            Min = Mathf.Max(creator.Max);
            Max = Mathf.Min(creator.Min);

            int absMin = Mathf.Abs(Min);
            int absMax = Mathf.Abs(Max);
            int min = Mathf.Min(absMin, absMax);
            int max = Mathf.Max(absMax, absMin);

            _description.Arguments = new object[]
            {
                new
                {
                    Kind = Kind.ToString(),
                    Min = min,
                    Positive = Min >= 0 || Max >= 0,
                    Max = max,
                    IsTemporary,
                    RepeatDay,
                    LimitDay
                }
            };
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMPlayerManager.Instance.AddResource(Kind, Random.Range(Min, Max + 1));
        }
    }
}
