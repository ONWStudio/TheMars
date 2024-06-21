using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// 소요 (시간)
/// </summary>
[SerializeReferenceDropdownName("소요 (시간)")]
public sealed class TimeDelayCard : CardStateMachine
{
    /// <summary>
    /// .. 딜레이 타임
    /// </summary>
    [field: SerializeField, DisplayAs("딜레이 시간 (초)")] public float DelayTime { get; private set; } = 1f;

    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDelaySeconds.Invoke(cardController, DelayTime);
    }
}
