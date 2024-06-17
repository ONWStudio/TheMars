using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RecycliingCard : CardStateMachine
{
    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnRecycleToHand.Invoke(cardController);
    }
}
