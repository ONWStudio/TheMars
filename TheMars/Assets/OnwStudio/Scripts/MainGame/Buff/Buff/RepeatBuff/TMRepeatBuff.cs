using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.AddressableAssets;
using Onw.Event;
using Onw.Attribute;
using TM.Manager;
using TM.Buff.Trigger;

namespace TM.Buff
{
    [System.Serializable]
    public abstract class TMRepeatBuff : TMBuffBase, ITMInitializeBuff<TMRepeatBuffTrigger>, IRemainingCountNotifier
    {
        [SerializeField, ReadOnly] private ReactiveField<int> _remainingDay = new();
        public IReadOnlyReactiveField<int> RemainingCount => _remainingDay;
        
        [field: SerializeField, ReadOnly] public int RepeatDay { get; set; }
        [field: SerializeField, ReadOnly] public int LimitDay { get; set; }
        [field: SerializeField, ReadOnly] public bool IsTemporary { get; set; }
        
        public virtual void Initialize(TMRepeatBuffTrigger creator)
        {
            RepeatDay = creator.RepeatDay;
            LimitDay = creator.LimitDay;
            IsTemporary = creator.IsTemporary;
        }

        protected sealed override void ApplyBuffProtected()
        {
            int dayCount = 0;
            _remainingDay.Value = LimitDay - dayCount;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

            void onChangedDay(int day)
            {
                dayCount++;
                _remainingDay.Value = LimitDay - dayCount;
                
                if (dayCount % RepeatDay == 0)
                {
                    OnChangedDayByRepeatDay(day);
                }

                if (!IsTemporary && dayCount == LimitDay)
                {
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                    Dispose();
                }
            }
        }

        protected abstract void OnChangedDayByRepeatDay(int day);

    }
}
