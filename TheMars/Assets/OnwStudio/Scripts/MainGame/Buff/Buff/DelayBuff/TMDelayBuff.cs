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
    public abstract class TMDelayBuff : TMBuffBase, ITMInitializeBuff<TMDelayBuffTrigger>, IRemainingCountNotifier
    {
        [field: SerializeField, ReadOnly] private ReactiveField<int> _remainingDay = new();

        public IReadOnlyReactiveField<int> RemainingCount => _remainingDay;

        [field: SerializeField, ReadOnly] public int DelayDayCount { get; set; }
        [field: SerializeField, ReadOnly] public bool IsTemporary { get; set; }

        protected virtual void OnApplyBuff() { }
        protected abstract void OnChangedDayByDelayCount(int day);

        public void Initialize(TMDelayBuffTrigger creator)
        {
            DelayDayCount = creator.DelayDayCount;
            IsTemporary = creator.IsTemporary;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _remainingDay.RemoveAllListener();
                _remainingDay = null;
            }
        }

        protected sealed override void ApplyBuffProtected()
        {
            int dayCount = 0;
            _remainingDay.Value = DelayDayCount - dayCount;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

            OnApplyBuff();

            void onChangedDay(int day)
            {
                dayCount++;
                _remainingDay.Value = DelayDayCount - dayCount;

                if (dayCount >= DelayDayCount && !IsTemporary)
                {
                    OnChangedDayByDelayCount(day);
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                    Dispose();
                }
            }
        }
    }
}