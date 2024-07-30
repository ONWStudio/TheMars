using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 지속 (시간)
    /// </summary>
    public sealed class TimeContinuingEffect : ITMCardSpecialEffect, ITMInitializableEffect<TimeContinuingEffectCreator>
    {
        /// <summary>
        /// .. 지속 시간
        /// </summary>
        [field: SerializeField, ReadOnly] public float ContinuingTime { get; private set; } = 1f;

        public string Label => "TimeContinuing";

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => TMCardGameManager.Instance.OnContinuingSeconds(
                controller,
                ContinuingTime,
                trigger.OnEffectEvent.Invoke));
        }

        public void Initialize(TimeContinuingEffectCreator effectCreator)
        {
            ContinuingTime = effectCreator.ContinuingTime;
        }
    }
}