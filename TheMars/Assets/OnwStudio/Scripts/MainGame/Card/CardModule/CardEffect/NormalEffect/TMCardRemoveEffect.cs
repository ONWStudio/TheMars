using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public sealed class TMCardRemoveEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            Debug.Log("카드 제거");
        }
    }
}