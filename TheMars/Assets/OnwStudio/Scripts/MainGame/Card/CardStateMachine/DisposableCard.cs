using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 일회용
/// </summary>
[SerializeReferenceDropdownName("일회용")]
public sealed class DisposableCard : CardStateMachine
{
    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDestroyCard.Invoke(cardController);
    }
}
