using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard;
using TMCard.Runtime;

namespace TMCard.Effect
{
    [SerializeReferenceDropdownName("카드 복사")]
    public sealed class TMCardCopyEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller)
        {
            Debug.Log("카드 카피");
        }
    }
}