using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    [SerializeReferenceDropdownName("카드 드로우")]
    public sealed class TMCardDrawEffect : ITMCardEffect
    {
        public void OnEffect(TMCardData cardData)
        {
            Debug.Log("카드 드로우");
        }
    }
}