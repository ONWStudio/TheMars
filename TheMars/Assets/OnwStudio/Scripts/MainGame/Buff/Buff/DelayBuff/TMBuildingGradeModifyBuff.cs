using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    public class TMBuildingGradeModifyBuff : TMDelayBuff
    {
        protected override AssetReferenceSprite IconReference
        {
            get;
            set;
        }

        protected override void OnApplyBuff()
        {
            
        }
        
        protected override void OnChangedDayByDelayCount(int day)
        {
        }
    }
}
