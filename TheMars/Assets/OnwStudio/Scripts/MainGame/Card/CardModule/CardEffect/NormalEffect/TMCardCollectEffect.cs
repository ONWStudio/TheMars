using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect
{
    [SerializeReferenceDropdownName("카드 획득")]
    public sealed class TMCardCollectEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller)
        {
            Debug.Log("카드 획득");
        }
    }
}