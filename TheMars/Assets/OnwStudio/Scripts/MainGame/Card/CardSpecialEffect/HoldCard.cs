using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnwAttributeExtensions;

/// <summary>
/// .. 보유
/// </summary>
[SerializeReferenceDropdownName("보유")]
public sealed class HoldCard : ICardSpecialEffect
{
    [field: SerializeField, DisplayAs("발동 트리거 카드"), Tooltip("보유 효과가 발동할때 참조할 카드 ID")]
    public string FriendlyCardID { get; private set; } = string.Empty;

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseStartedState = null;
        cardController.UseEndedState = null;
    }
}
