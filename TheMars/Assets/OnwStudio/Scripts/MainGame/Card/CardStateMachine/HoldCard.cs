using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HoldCard : CardStateMachine
{
    public int FriendlyCardID { get; internal set; } = 0;

    public override void OnUseStarted<T>(T cardController) {}
    public override void OnUseEnded<T>(T cardController) {}

    public override void OnDrawBegin<T>(T cardController)
    {
        cardController.OnHoldCard.Invoke(cardController, FriendlyCardID);
    }
}
