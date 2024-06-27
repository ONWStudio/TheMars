using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 신기루
/// </summary>
[SerializeReferenceDropdownName("신기루")]
public sealed class MirageCard : ICardSpecialEffect
{
    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseState = () => cardController.OnDisposableCard.Invoke(cardController);
        cardController.TurnEndedState = () => cardController.OnDestroyCard.Invoke(cardController);
    }
}
