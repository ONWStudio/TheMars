using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 소요 (시간)
    public sealed class TimeDelayEffect : TMCardSpecialEffect, ITMInitializableEffect<TimeDelayEffectCreator>
    {
        /// <summary>
        /// .. 딜레이 타임
        /// </summary>
        [field: SerializeField, DisplayAs("딜레이 시간 (초)"), ReadOnly] public float DelayTime { get; private set; } = 1f;

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            // .. TODO : 수정
            controller.OnClickEvent.RemoveAllToAddListener(() => TMCardHelper.Instance.DelaySeconds(controller, DelayTime));
        }

        public void Initialize(TimeDelayEffectCreator effectCreator)
        {
            DelayTime = effectCreator.DelayTime;
        }

        public TimeDelayEffect() : base("TurnDelay") {}
    }
}
