using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 지속 (턴)
    /// </summary>
    public sealed class TurnContinuingEffect : TMCardSpecialEffect, ITMInitializableEffect<TurnContinuingEffectCreator>
    {
        /// <summary>
        /// .. 지속 할 턴
        /// </summary>
        [field: SerializeField, DisplayAs("지속 턴")] public int ContinuingTurn { get; private set; } = 1;

        public void Initialize(TurnContinuingEffectCreator effectCreator)
        {
            ContinuingTurn = effectCreator.ContinuingTurn;
        }

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => onContinuingTurns(controller, ContinuingTurn));

            static void onContinuingTurns(TMCardController card, int turn)
            {
                //DelayEffectManager.Instance.WaitForTurnCountEffect(
                //    turn,
                //    () => { },
                //    turnCount => cardUI.CardData.UseCard());

                //MoveToTombAndHide(cardUI);
            }
        }

        public TurnContinuingEffect() : base("TurnContinuing") {}
    }
}
