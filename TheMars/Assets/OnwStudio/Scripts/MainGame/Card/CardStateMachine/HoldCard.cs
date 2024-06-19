using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HoldCard : CardStateMachine
{
    [field: SerializeField, Tooltip("보유 효과가 발동할때 참조할 카드 ID")] public string FriendlyCardID { get; private set; } = string.Empty;

    public override void OnUseStarted<T>(T cardController) {}
    public override void OnUseEnded<T>(T cardController) {}

    public override void OnDrawBegin<T>(T cardController)
    {
        cardController.OnHoldCard.Invoke(cardController, FriendlyCardID);
    }
}
