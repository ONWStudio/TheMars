using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 소요 (턴)
/// </summary>
[SerializeReferenceDropdownName("소요 (턴)")]
public sealed class TurnDelayCard : ICardSpecialEffect
{
    /// <summary>
    /// .. 소요시킬 턴
    /// </summary>
    [field: SerializeField, DisplayAs("소요 턴")] public int DelayTurn { get; private set; } = 5;

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseState = () =>
            cardController.OnDelayTurn.Invoke(cardController, DelayTurn);
    }
}
