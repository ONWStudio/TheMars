using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 버리기
/// </summary>
[SerializeReferenceDropdownName("버리기")]
public sealed class DropCard : ICardSpecialEffect
{
    /// <summary>
    /// .. 버리기 효과
    /// </summary>
    public IReadOnlyList<ICardEffect> CardEffects => _cardEffects;

    [SerializeReference, DisplayAs("버리기 효과"), SerializeReferenceDropdown] private List<ICardEffect> _cardEffects = new();

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.TurnEndedState = () =>
        {
            _cardEffects.ForEach(cardEffect => cardEffect.OnEffect(cardController.gameObject, cardController.CardData));
            cardController.OnMoveToScreenCenter.Invoke(cardController, false);
        };
    }
}
