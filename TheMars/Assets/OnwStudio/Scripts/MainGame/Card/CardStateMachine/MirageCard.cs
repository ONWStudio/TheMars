using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MirageCard : CardStateMachine
{
    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDestroyCard.Invoke(cardController);
    }

    public override void OnTurnEnd<T>(T cardController)
    {
        cardController.OnDestroyCard.Invoke(cardController);
    }
}
