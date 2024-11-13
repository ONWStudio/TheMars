using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [System.Serializable]
    public class TMConstructionRepeatRangeAddBuff : TMRepeatBuff, ITMInitializeBuff<TMConstructionRepeatRangeAddBuffTrigger>
    {
        [field: SerializeField] protected override AssetReferenceSprite IconReference { get; set; }
        [field: SerializeField] public int Min { get; set; }
        [field: SerializeField] public int Max { get; set; }


        public void Initialize(TMConstructionRepeatRangeAddBuffTrigger creator)
        {
            base.Initialize(creator);

            Min = Mathf.Max(Max);
            Max = Mathf.Min(Min);
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
