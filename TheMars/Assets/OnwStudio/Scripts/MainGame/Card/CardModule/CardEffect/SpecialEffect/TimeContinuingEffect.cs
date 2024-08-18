using Onw.Attribute;
using Onw.ServiceLocator;
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
            controller.OnClickEvent.RemoveAllToAddListener(() => onContinuingSeconds(
                controller,
                ContinuingTime,
                trigger.OnEffectEvent.Invoke));

            static void onContinuingSeconds(TMCardController card, float continuingSeconds, System.Action onSuccess)
            {
                if (!ServiceLocator<ITMCardService>.TryGetService(out var service) ||
                    !ServiceLocator<TMDelayEffectManager>.TryGetService(out var delayEffectManager)) return;

                delayEffectManager.WaitForSecondsEffect(
                    continuingSeconds,
                    () =>
                    {
                        Debug.Log("지속 시간 종료");
                        onSuccess.Invoke();
                    },
                    remainingTime => { });

                card.MoveToTombAndHide();
            }
        }

        public void Initialize(TimeContinuingEffectCreator effectCreator)
        {
            ContinuingTime = effectCreator.ContinuingTime;
        }

        public TimeContinuingEffect() : base("TimeContinuing") {}
    }
}