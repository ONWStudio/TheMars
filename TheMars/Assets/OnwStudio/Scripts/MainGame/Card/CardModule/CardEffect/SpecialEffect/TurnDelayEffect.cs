using Onw.Attribute;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 소요 (턴)
    /// </summary>
    public sealed class TurnDelayEffect : TMCardSpecialEffect, ITMInitializeEffect<TurnDelayEffectCreator>
    {
        /// <summary>
        /// .. 소요시킬 턴
        /// </summary>
        [field: SerializeField, DisplayAs("소요 턴"), ReadOnly] public int DelayTurnCount { get; private set; } = 5;

        public void Initialize(TurnDelayEffectCreator effectCreator)
        {
            DelayTurnCount = effectCreator.DelayTurn;
        }

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(eventState => delayTurn(controller, DelayTurnCount));
        }

        private static void delayTurn(TMCardController card, int turnCount)
        {
            if (!ServiceLocator<TMDelayEffectManager>.TryGetService(out TMDelayEffectManager service)) return;
            
            service.WaitForTurnCountEffect(
                turnCount,
                card.SetActiveDelayCard,
                remainingTurn => { });

            card.MoveToTombAndHide();
        }
        public TurnDelayEffect() : base("TurnDelay") {}
    }
}
