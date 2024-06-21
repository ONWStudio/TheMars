using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 버리기
/// </summary>
[SerializeReferenceDropdownName("버리기")]
public sealed class DropCard : CardStateMachine
{
    /// <summary>
    /// .. 버리기 효과
    /// </summary>
    public IReadOnlyList<ICardEffect> CardEffects => _cardEffects;

    [SerializeField, DisplayAs("버리기 효과")] private List<ICardEffect> _cardEffects = new();

    public override void OnTurnEnd<T>(T cardController)
    {
        _cardEffects
            .ForEach(cardEffect => cardEffect.OnEffect(cardController.gameObject, cardController.CardData));

        base.OnTurnEnd(cardController);
    }
}
