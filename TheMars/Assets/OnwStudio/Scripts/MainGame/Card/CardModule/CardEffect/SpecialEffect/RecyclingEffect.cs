using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;
using TMCard;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 재활용
    /// </summary>
    public sealed class RecyclingEffect : ITMCardSpecialEffect
    {
        public string Label => "Recycling";

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                TMCardGameManager.Instance.RecycleToHand(controller);
            });
        }
    }
}
