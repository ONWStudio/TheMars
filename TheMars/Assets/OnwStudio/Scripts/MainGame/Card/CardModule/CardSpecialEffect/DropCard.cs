using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Interface;
using TMCard.UI;
using TMCard.Effect;

namespace TMCard.SpecialEffect
{
    using UI;
    using Effect;

    /// <summary>
    /// .. 버리기
    /// </summary>
    [SerializeReferenceDropdownName("버리기")]
    public sealed class DropCard : ITMCardSpecialEffect, IDescriptable
    {
        public int No => 3;

        public string Description => _cardEffects
            .OfType<IDescriptable>()
            .BuildToString();

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