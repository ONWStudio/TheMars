using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public sealed class TMCardDropEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller)
        {
            Debug.Log("카드 버리기");
        }
    }
}