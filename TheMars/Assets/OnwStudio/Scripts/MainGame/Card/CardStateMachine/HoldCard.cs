using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HoldCard : CardStateMachine
{
    [field: SerializeField] public string FriendlyCardID { get; private set; } = string.Empty;

    public override void OnUseStarted<T>(T cardController) {}
    public override void OnUseEnded<T>(T cardController) {}

    public override void OnDrawBegin<T>(T cardController)
    {
        cardController.OnHoldCard.Invoke(cardController, FriendlyCardID);
    }
}
