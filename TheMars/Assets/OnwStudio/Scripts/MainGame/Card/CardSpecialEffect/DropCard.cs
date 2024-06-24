using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 버리기
/// </summary>
[SerializeReferenceDropdownName("버리기")]
public sealed class DropCard : ICardSpecialEffect
{
    /// <summary>
    /// .. 버리기 효과
    /// </summary>
    public IReadOnlyList<ICardEffect> CardEffects => _cardEffects;

    [SerializeReference, DisplayAs("버리기 효과"), SerializeReferenceDropdown] private List<ICardEffect> _cardEffects = new();

    public void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        throw new System.NotImplementedException();
    }

    public bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects)
    {
        throw new System.NotImplementedException();
    }
}
