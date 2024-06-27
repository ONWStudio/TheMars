using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeReferenceDropdownName("카드 버리기")]
public sealed class TMCardDropEffect : ITMCardEffect
{
    public void OnEffect(TMCardData cardData)
    {
        Debug.Log("카드 버리기");
    }
}
