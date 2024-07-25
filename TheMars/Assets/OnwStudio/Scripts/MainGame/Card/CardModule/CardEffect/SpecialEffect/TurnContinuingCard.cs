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
    [SerializeReferenceDropdownName("지속 (턴)"), Substitution("지속(턴)")]
    public sealed class TurnContinuingCard : ITMCardSpecialEffect
    {
        /// <summary>
        /// .. 지속 할 턴
        /// </summary>
        [field: SerializeField, DisplayAs("지속 턴")] public int ContinuingTurn { get; private set; } = 1;

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("지속(턴)");

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.OnContinuingTurns(cardController, ContinuingTurn);
        }
    }
}
