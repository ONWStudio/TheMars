using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 신기루
/// </summary>
[SerializeReferenceDropdownName("신기루")]
public sealed class MirageCard : CardStateMachine
{
    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDestroyCard.Invoke(cardController);
    }

    public override void OnTurnEnd<T>(T cardController)
    {
        cardController.OnDestroyCard.Invoke(cardController);
    }
}
