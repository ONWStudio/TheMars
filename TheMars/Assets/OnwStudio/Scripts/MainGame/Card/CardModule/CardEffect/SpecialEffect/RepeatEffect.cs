using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Interface;
using TMCard.Runtime;
using TMCard.Effect;
using TMCard.Effect.Resource;
using TMCard;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 반복
    /// </summary>
    public sealed class RepeatEffect : TMCardSpecialEffect
    {
        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();

                controller
                    .Effects
                    .OfType<ITMCardResourceEffect>()
                    .ForEach(resourceEffect => resourceEffect.AddRewardResource(resourceEffect.Amount));

                TMCardGameManager.Instance.MoveToScreenCenterAfterToTomb(controller);
            });
        }

        public RepeatEffect() : base("Repeat") {}
    }
}