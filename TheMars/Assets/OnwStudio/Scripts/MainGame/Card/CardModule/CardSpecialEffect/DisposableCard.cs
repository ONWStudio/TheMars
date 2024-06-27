using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// .. 일회용
/// </summary>
[SerializeReferenceDropdownName("일회용")]
public sealed class DisposableCard : ICardSpecialEffect
{
    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseState = () =>
        {
            cardController.CardData.UseCard();
            cardController.OnDisposableCard.Invoke(cardController);
        };
    }
}
