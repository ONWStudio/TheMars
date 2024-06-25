using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 지속 (턴)
/// </summary>
[SerializeReferenceDropdownName("지속 (턴)")]
public sealed class TurnContinuingCard : ICardSpecialEffect
{
    /// <summary>
    /// .. 지속 할 턴
    /// </summary>
    [field: SerializeField, DisplayAs("지속 턴")] public int ContinuingTurn { get; private set; } = 1;

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
    }
}
