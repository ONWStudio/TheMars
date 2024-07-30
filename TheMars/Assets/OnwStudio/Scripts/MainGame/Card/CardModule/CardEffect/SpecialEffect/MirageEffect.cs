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
    public sealed class MirageEffect : ITMCardSpecialEffect
    {
        public string Label => "Mirage";

        public void ApplyEffect(TMCardController cardController)
        {
            //cardController.UseState = () => TMCardGameManager.Instance.DisposeCard(cardController);
            //cardController.TurnEndedState = () => TMCardGameManager.Instance.DestroyCard(cardController);
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                TMCardGameManager.Instance.DisposeCard(controller);
            });

            controller.OnTurnEndedEvent.RemoveAllToAddListener(() => TMCardGameManager.Instance.DestroyCard(controller));
        }
    }
}
