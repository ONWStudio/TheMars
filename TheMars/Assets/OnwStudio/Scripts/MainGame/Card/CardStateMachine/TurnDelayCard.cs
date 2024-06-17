using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TurnDelayCard : CardStateMachine
{
    public int DelayTurnCount { get; internal set; } = 5;

    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDelayTurn.Invoke(cardController, DelayTurnCount);
    }
}
