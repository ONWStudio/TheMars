using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 지속 (시간)
    /// </summary>
    public sealed class TimeContinuingEffect : TMCardSpecialEffect, ITMInitializableEffect<TimeContinuingEffectCreator>
    {
        /// <summary>
        /// .. 지속 시간
        /// </summary>
        [field: SerializeField, ReadOnly] public float ContinuingTime { get; private set; } = 1f;

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => TMCardHelper.Instance.OnContinuingSeconds(
                controller,
                ContinuingTime,
                trigger.OnEffectEvent.Invoke));
        }

        public void Initialize(TimeContinuingEffectCreator effectCreator)
        {
            ContinuingTime = effectCreator.ContinuingTime;
        }

        public TimeContinuingEffect() : base("TimeContinuing") {}
    }
}