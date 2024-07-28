using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 일회용
    /// </summary>
    public sealed class DisposableEffect : ITMCardSpecialEffect
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("일회용");

        public void ApplyEffect(TMCardController cardController)
        {
            //cardController.UseState = () =>
            //{
            //    cardController.CardData.ApplyEffect();
            //    TMCardGameManager.Instance.DisposeCard(cardController);
            //};
        }
    }
}
