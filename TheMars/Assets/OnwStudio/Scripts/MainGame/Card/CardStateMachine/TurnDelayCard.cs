using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TurnDelayCard : CardStateMachine
{
    [field: SerializeField] public int DelayTurnCount { get; private set; } = 5;

    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDelayTurn.Invoke(cardController, DelayTurnCount);
    }
}
