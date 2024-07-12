using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 지속 (턴)
    /// </summary>
    [SerializeReferenceDropdownName("지속 (턴)")]
    public sealed class TurnContinuingCard : ICardSpecialEffect
    {
        /// <summary>
        /// .. 지속 할 턴
        /// </summary>
        [field: SerializeField, DisplayAs("지속 턴")] public int ContinuingTurn { get; private set; } = 1;

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.OnContinuingTurns(cardController, ContinuingTurn);
        }
    }
}
