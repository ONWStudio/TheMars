using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 지속 (시간)
/// </summary>
[SerializeReferenceDropdownName("지속 (시간)")]
public sealed class TimeContinuingCard : ICardSpecialEffect
{
    /// <summary>
    /// .. 지속 시간
    /// </summary>
    [field: SerializeField, DisplayAs("지속 시간")] public float ContinuingTime { get; private set; } = 1f;

    public void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        throw new System.NotImplementedException();
    }

    public bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects)
    {
        throw new System.NotImplementedException();
    }
}
