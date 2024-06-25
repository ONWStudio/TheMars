using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 재활용
/// </summary>
[SerializeReferenceDropdownName("재활용")]
public sealed class RecyclingCard : ICardSpecialEffect
{
    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseEndedState = () => cardController.OnRecycleToHand.Invoke(cardController);
    }
}
