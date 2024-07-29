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
    public sealed class RepeatEffect : ITMCardSpecialEffect
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("반복");

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                controller.ResourceEffects.ForEach(resourceEffect => resourceEffect.AddRequiredResource(resourceEffect.Amount));
                TMCardGameManager.Instance.MoveToScreenCenterAfterToTomb(controller);
            });
        }
    }
}