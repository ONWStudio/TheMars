using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DisposableCard : CardStateMachine
{
    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDestroyCard.Invoke(cardController);
    }
}
