using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;
using TMCard;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 신기루
    /// </summary>
    public sealed class MirageEffect : TMCardSpecialEffect
    {
        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                TMCardHelper.Instance.DisposeCard(controller);
            });

            controller.OnTurnEndedEvent.RemoveAllToAddListener(() => TMCardHelper.Instance.DestroyCard(controller));
        }

        public MirageEffect() : base("Mirage") { }
    }
}
