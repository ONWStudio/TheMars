using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect
{
    [SerializeReferenceDropdownName("카드 발견")]
    public sealed class TMCardSelectEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller)
        {
            Debug.Log("카드 발견");
        }
    }
}
