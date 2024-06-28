using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    [SerializeReferenceDropdownName("카드 복사")]
    public sealed class TMCardCopyEffect : ITMCardEffect
    {
        public void OnEffect(TMCardData cardData)
        {
            Debug.Log("카드 카피");
        }
    }
}