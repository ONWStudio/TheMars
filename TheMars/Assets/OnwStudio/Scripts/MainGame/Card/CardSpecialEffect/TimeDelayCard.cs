using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// 소요 (시간)
/// </summary>
[SerializeReferenceDropdownName("소요 (시간)")]
public sealed class TimeDelayCard : ICardSpecialEffect
{
    /// <summary>
    /// .. 딜레이 타임
    /// </summary>
    [field: SerializeField, DisplayAs("딜레이 시간 (초)")] public float DelayTime { get; private set; } = 1f;

    public void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
    }

    public bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects)
    {
        return true;
    }
}
