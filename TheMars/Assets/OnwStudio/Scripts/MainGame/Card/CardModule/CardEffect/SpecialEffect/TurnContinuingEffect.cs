using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 지속 (턴)
    /// </summary>
    public sealed class TurnContinuingEffect : ITMCardSpecialEffect, ITMInitializableEffect<TurnContinuingEffectCreator>
    {
        /// <summary>
        /// .. 지속 할 턴
        /// </summary>
        [field: SerializeField, DisplayAs("지속 턴")] public int ContinuingTurn { get; private set; } = 1;

        public string Label => "TurnContinuing";

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => TMCardGameManager.Instance.OnContinuingTurns(controller, ContinuingTurn));
        }

        public void Initialize(TurnContinuingEffectCreator effectCreator)
        {
            ContinuingTurn = effectCreator.ContinuingTurn;
        }
    }
}
