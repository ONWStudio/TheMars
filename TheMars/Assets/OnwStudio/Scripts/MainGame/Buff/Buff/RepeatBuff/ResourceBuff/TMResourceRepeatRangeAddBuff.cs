using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [System.Serializable]
    public class TMResourceRepeatRangeAddBuff : TMResourceRepeatAddBuffBase, ITMInitializeBuff<TMResourceRepeatRangeAddBuffTrigger>
    {
        protected override AssetReferenceSprite IconReference { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        [field: SerializeField] public int Min { get; set; }
        [field: SerializeField] public int Max { get; set; }

        public void Initialize(TMResourceRepeatRangeAddBuffTrigger creator)
        {
            base.Initialize(creator);

            Min = Mathf.Max(creator.Max);
            Max = Mathf.Min(creator.Min);
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMPlayerManager.Instance.AddResource(ResourceKind, Random.Range(Min, Max + 1));
        }
    }
}
