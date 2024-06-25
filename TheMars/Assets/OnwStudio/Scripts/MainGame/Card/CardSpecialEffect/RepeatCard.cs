using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 반복
/// </summary>
[SerializeReferenceDropdownName("반복")]
public sealed class RepeatCard : ICardSpecialEffect
{

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
    }
}
