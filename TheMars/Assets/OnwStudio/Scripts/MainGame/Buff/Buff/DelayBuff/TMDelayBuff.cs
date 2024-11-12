using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Event;
using TM.Buff.Trigger;
using TM.Manager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace TM.Buff
{
    [System.Serializable]
    public abstract class TMDelayBuff : TMBuffBase, ITMInitializeBuff<TMDelayBuffTrigger>, IAccrueDayNotifier
    {
        [field: SerializeField, ReadOnly] private ReactiveField<int> _accrueDay = new();

        public IReadOnlyReactiveField<int> AccrueDay => _accrueDay;
        
        [field: SerializeField, ReadOnly] public int DelayDayCount { get; private set; }
        
        protected virtual void OnApplyBuff() { }
        protected abstract void OnChangedDayByDelayCount(int day);
        
        public void Initialize(TMDelayBuffTrigger creator)
        {
            DelayDayCount = creator.DelayDayCount;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _accrueDay.RemoveAllListener();
                _accrueDay = null;
            }
        }
        
        protected sealed override void ApplyBuffProtected()
        {
            _accrueDay.Value = 0;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            
            OnApplyBuff();

            void onChangedDay(int day)
            {
                _accrueDay.Value++;

                if (_accrueDay.Value >= DelayDayCount)
                {
                    OnChangedDayByDelayCount(day);
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                    Dispose();
                }
            }
        }
    }
}
