using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 재활용
/// </summary>
[SerializeReferenceDropdownName("재활용")]
public sealed class RecycliingCard : CardStateMachine
{
    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnRecycleToHand.Invoke(cardController);
    }
}
