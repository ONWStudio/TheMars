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
    public class TMConstructionRepeatRangeAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionRepeatRangeAddBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Construction_Repeat_Range_Add_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField] public int Min { get; set; }
        [field: SerializeField] public int Max { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMConstructionRepeatRangeAddBuffTrigger creator)
        {
            base.Initialize(creator);

            Min = Mathf.Max(creator.Max);
            Max = Mathf.Min(creator.Min);

            int absMin = Mathf.Abs(Min);
            int absMax = Mathf.Abs(Max);

            int min = Mathf.Min(absMin, absMax);
            int max = Mathf.Max(absMin, absMax);

            _description.Arguments = new object[]
            {
                new
                {
                    RepeatDay,
                    LimitDay,
                    IsTemporary,
                    Min = min,
                    Max = max,
                    Positive = Min >= 0 || Max >= 0
                }
            };
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            int resourceAdd = Random.Range(Min, Max + 1);
            TMPlayerManager.Instance.Steel.Value += resourceAdd;
            TMPlayerManager.Instance.Plants.Value += resourceAdd;
            TMPlayerManager.Instance.Clay.Value += resourceAdd;
        }
    }
}
