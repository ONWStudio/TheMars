using System.Collections;
using System.Collections.Generic;
using OnwAttributeExtensions;
using UnityEngine;

public abstract class TMCardResourceEffect : ITMCardEffect
{
    [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화")] public int Amount { get; private set; }

    public void OnEffect(TMCardData cardData)
    {
        OnResourceEffect(cardData);
    }

    /// <summary>
    /// .. 외부에서 TMCardResourceEffect에 직접 접근하여 쓰는 경우가 있을때 사용하는 메서드 입니다
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="addtionalAmount"></param>
    public abstract void OnResourceEffect(TMCardData cardData, int addtionalAmount = 0);
}
