using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using Onw.Event;
using TM.Manager;
using TM.Buff.Trigger;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TM.Buff
{
    [System.Serializable]
    public abstract class TMRepeatBuff : TMBuffBase, ITMInitializeBuff<TMRepeatBuffTrigger>, IAccrueDayNotifier
    {
        [SerializeField, ReadOnly] private ReactiveField<int> _accrueDay = new();
        public IReadOnlyReactiveField<int> AccrueDay => _accrueDay;
        
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
            _accrueDay.Value = 0;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

            void onChangedDay(int day)
            {
                _accrueDay.Value++;
                
                if (_accrueDay.Value % RepeatDay == 0)
                {
                    OnChangedDayByRepeatDay(day);
                }

                if (!IsTemporary && _accrueDay.Value == LimitDay)
                {
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                    Dispose();
                }
            }
        }

        protected abstract void OnChangedDayByRepeatDay(int day);

    }
}
