using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 반복
/// </summary>
[SerializeReferenceDropdownName("반복")]
public sealed class RepeatCard : ICardSpecialEffect
{
    public void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        throw new System.NotImplementedException();
    }

    public bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects)
    {
        throw new System.NotImplementedException();
    }
}
