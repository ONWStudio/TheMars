using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public sealed class TMCardCopyEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() => 
            {
                Debug.Log("카드 카피");
            });
        }
    }
}