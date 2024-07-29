using System.Collections;
using System.Collections.Generic;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect
{
    public sealed class TMCardDrawEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            Debug.Log("카드 드로우");
        }
    }
}