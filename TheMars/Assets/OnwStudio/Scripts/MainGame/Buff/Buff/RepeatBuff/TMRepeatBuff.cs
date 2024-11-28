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
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public abstract class TMRepeatBuff : TMBuffBase, ITMInitializeBuff<TMRepeatBuffTrigger>, IRemainingCountNotifier
    {
        [SerializeField, ReadOnly] private ReactiveField<int> _remainingDay = new();
        [SerializeField, ReadOnly] private ReactiveField<int> _remainingDayByNextEffect = new();
        
        public IReadOnlyReactiveField<int> RemainingCount => _remainingDay;
        public IReadOnlyReactiveField<int> RemainingDayByNextEffect => _remainingDayByNextEffect;
        
        [field: SerializeField, ReadOnly] public int RepeatDay { get; set; }
        [field: SerializeField, ReadOnly] public int LimitDay { get; set; }
        [field: SerializeField, ReadOnly] public bool IsTemporary { get; set; }

        [field: SerializeField, ReadOnly] public LocalizedString RepeatTimeDescription { get; private set; } = new("TM_UI", "Repeat_Buff_Time_Description");
        
        public virtual void Initialize(TMRepeatBuffTrigger creator)
        {
            RepeatDay = creator.RepeatDay;
            LimitDay = creator.LimitDay;
            IsTemporary = creator.IsTemporary;
        }

        protected sealed override void ApplyBuffProtected()
        {
            int dayCount = 0;
            _remainingDay.Value = LimitDay;
            _remainingDayByNextEffect.Value = RepeatDay;
            TMSimulator.Instance.NowDay.AddListenerWithoutNotify(onChangedDay);

            void onChangedDay(int day)
            {
                dayCount++;
                int cumulativeDay = dayCount % RepeatDay;
                _remainingDay.Value = LimitDay - dayCount;
                _remainingDayByNextEffect.Value = RepeatDay - cumulativeDay;
                
                if (cumulativeDay == 0)
                {
                    OnChangedDayByRepeatDay(day);
                }

                if (!IsTemporary && dayCount == LimitDay)
                {
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                    Dispose();
                }
                
                setArguments();
            }
        }
        
        private void setArguments()
        {
            RepeatTimeDescription.Arguments = new object[]
            {
                new
                {
                    Next = _remainingDayByNextEffect.Value,
                    RemainingDay = RemainingCount.Value,
                    IsTemporary
                }
            };
        }

        protected abstract void OnChangedDayByRepeatDay(int day);

    }
}
