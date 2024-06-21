using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 지속 (턴)
/// </summary>
[SerializeReferenceDropdownName("지속 (턴)")]
public sealed class TurnContinuingCard : CardStateMachine
{
    /// <summary>
    /// .. 지속 할 턴
    /// </summary>
    [field: SerializeField, DisplayAs("지속 턴")] public int ContinuingTurn { get; private set; } = 1;

    public override void OnUseStarted<T>(T cardController)
    {
        base.OnUseStarted(cardController);
    }
}
