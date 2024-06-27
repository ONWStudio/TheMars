using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeReferenceDropdownName("카드 획득")]
public sealed class TMCardCollectEffect : ITMCardEffect
{
    public void OnEffect(TMCardData cardData)
    {
        Debug.Log("카드 획득");
    }
}
