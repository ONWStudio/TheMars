using Onw.Attribute;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 소요 (시간)
    /// </summary>
    public sealed class TimeDelayEffect : TMCardSpecialEffect, ITMCardInitializeEffect<TimeDelayEffectCreator>
    {
        /// <summary>
        /// .. 딜레이 타임
        /// </summary>
        [field: SerializeField, DisplayAs("딜레이 시간 (초)"), ReadOnly] public float DelayTime { get; private set; } = 1f;

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            // .. TODO : 수정
            controller.OnClickEvent.RemoveAllToAddListener(eventState => delaySeconds(controller, DelayTime));

            static void delaySeconds(TMCardController card, float delayTime)
            {
                if (!ServiceLocator<TMDelayEffectManager>.TryGetService(out TMDelayEffectManager service)) return;

                service.WaitForSecondsEffect(
                    delayTime,
                    card.SetActiveDelayCard,
                    remainingTime => { });

                card.MoveToTombAndHide();
            }
        }

        public void Initialize(TimeDelayEffectCreator effectCreator)
        {
            DelayTime = effectCreator.DelayTime;
        }

        public TimeDelayEffect() : base("TurnDelay") {}
    }
}
