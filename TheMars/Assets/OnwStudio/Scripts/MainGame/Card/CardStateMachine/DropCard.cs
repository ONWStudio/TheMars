using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DropCard : CardStateMachine
{
    public IReadOnlyList<ICardEffect> CardEffects => _cardEffects;

    [SerializeField] private List<ICardEffect> _cardEffects = new();

    public override void OnTurnEnd<T>(T cardController)
    {
        _cardEffects
            .ForEach(cardEffect => cardEffect.OnEffect(cardController.gameObject, cardController.CardData));

        base.OnTurnEnd(cardController);
    }
}
