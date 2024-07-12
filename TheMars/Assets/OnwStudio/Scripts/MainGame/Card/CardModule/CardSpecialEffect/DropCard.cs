using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    using UI;
    using Effect;

    /// <summary>
    /// .. 버리기
    /// </summary>
    [SerializeReferenceDropdownName("버리기")]
    public sealed class DropCard : ICardSpecialEffect
    {
        /// <summary>
        /// .. 버리기 효과
        /// </summary>
        public IReadOnlyList<ITMCardEffect> CardEffects => _cardEffects;

        [SerializeReference, DisplayAs("버리기 효과"), SerializeReferenceDropdown] private List<ITMCardEffect> _cardEffects = new();

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.TurnEndedState = () =>
            {
                _cardEffects.ForEach(cardEffect => cardEffect.OnEffect(cardController.CardData));
                TMCardGameManager.Instance.DisposeCard(cardController);
            };
        }
    }
}