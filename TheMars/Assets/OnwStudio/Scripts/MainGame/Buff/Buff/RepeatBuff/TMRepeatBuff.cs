using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using TM.Manager;
using TM.Buff.Trigger;
using UnityEngine.Events;

namespace TM.Buff
{
    public abstract class TMRepeatBuff : TMBuffBase, ITMInitializeBuff<TMRepeatBuffTrigger>
    {
        public event UnityAction<TMRepeatBuff> OnTriggerRepeatBuff
        {
            add => _onTriggerRepeatBuff.AddListener(value);
            remove => _onTriggerRepeatBuff.RemoveListener(value);
        }

        [field: SerializeField, ReadOnly] public int RepeatDay { get; set; }
        [field: SerializeField, ReadOnly] public int LimitDay { get; set; }
        [field: SerializeField, ReadOnly] public bool IsTemporary { get; set; }

        [field: SerializeField] private UnityEvent<TMRepeatBuff> _onTriggerRepeatBuff = new();

        public virtual void Initialize(TMRepeatBuffTrigger creator)
        {
            RepeatDay = creator.RepeatDay;
            LimitDay = creator.LimitDay;
            IsTemporary = creator.IsTemporary;
        }

        protected override sealed void ApplyBuffProtected()
        {
            int dayCount = 0;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount % RepeatDay == 0)
                {
                    OnChangedDayByRepeatDay(day);
                }

                if (!IsTemporary && dayCount == LimitDay)
                {
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                }
            }
        }

        protected abstract void OnChangedDayByRepeatDay(int day);
    }
}
