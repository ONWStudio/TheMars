using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TimeContinuingCard : CardStateMachine
{
    public override void OnUseStarted<T>(T cardController)
    {
        base.OnUseStarted(cardController);
    }
}
