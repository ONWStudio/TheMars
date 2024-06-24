using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 일회용
/// </summary>
[SerializeReferenceDropdownName("일회용")]
public sealed class DisposableCard : ICardSpecialEffect
{
    public void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {

    }

    public bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects)
    {
        return true;
    }
}
