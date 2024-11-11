using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Buff.Trigger;
using TM.Manager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace TM.Buff
{
    public abstract class TMDelayBuff : TMBuffBase, ITMInitializeBuff<TMDelayBuffTrigger>
    {
        [field: SerializeField, ReadOnly] public int DelayDayCount { get; private set; }
        
        protected virtual void OnApplyBuff() { }
        protected abstract void OnChangedDayByDelayCount(int day);
        
        public void Initialize(TMDelayBuffTrigger creator)
        {
            DelayDayCount = creator.DelayDayCount;
        }
        
        protected sealed override void ApplyBuffProtected()
        {
            int dayCount = 0;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            
            OnApplyBuff();

            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount >= DelayDayCount)
                {
                    OnChangedDayByDelayCount(day);
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                    Dispose();
                }
            }
        }
    }
}
