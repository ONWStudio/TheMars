using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 드로우
/// </summary>
[SerializeReferenceDropdownName("드로우")]
public sealed class DrawCard : ICardSpecialEffect
{
    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseEndedState = () => cardController.OnRecycleToHand.Invoke(cardController);
    }
}
