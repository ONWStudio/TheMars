using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DrawCard : CardStateMachine
{
    public override void OnUseStarted<T>(T cardController) {}
    public override void OnUseEnded<T>(T cardController) {}
    public override void OnTurnEnd<T>(T cardController) {}

    public override void OnDrawBegin<T>(T cardController)
    {
        cardController.CardData.UseCard(cardController.gameObject);
    }

    public override void OnDrawEnded<T>(T cardController)
    {
        cardController.OnDrawUse.Invoke(cardController);
    }
}
