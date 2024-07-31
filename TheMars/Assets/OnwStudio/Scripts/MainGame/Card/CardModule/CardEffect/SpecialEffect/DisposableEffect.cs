using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine.Localization;
using Onw.Localization;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 일회용
    /// </summary>
    public sealed class DisposableEffect : TMCardSpecialEffect
    {
        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllListener();
            controller.OnClickEvent.AddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                TMCardGameManager.Instance.DisposeCard(controller);
            });
        }

        public DisposableEffect() : base("Disposable") { }
    }
}
