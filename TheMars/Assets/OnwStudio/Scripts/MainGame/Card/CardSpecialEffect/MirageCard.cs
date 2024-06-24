using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 신기루
/// </summary>
[SerializeReferenceDropdownName("신기루")]
public sealed class MirageCard : ICardSpecialEffect
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
