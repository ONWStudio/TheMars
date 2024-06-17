using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DropCard : CardStateMachine
{
    public List<ICardEffect> CardEffects { get; internal set; } = new();

    public override void OnTurnEnd<T>(T cardController)
    {
        CardEffects
            .ForEach(cardEffect => cardEffect.OnEffect(cardController.gameObject, cardController.CardData));

        base.OnTurnEnd(cardController);
    }
}
