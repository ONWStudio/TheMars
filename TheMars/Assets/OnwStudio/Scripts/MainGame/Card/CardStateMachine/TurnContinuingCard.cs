using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TurnContinuingCard : CardStateMachine
{
    public override void OnUseStarted<T>(T cardController)
    {
        base.OnUseStarted(cardController);
    }
}
