using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard;

namespace TMCard.Effect
{
    using Onw.Interface;
    using Runtime;

    /// <summary>
    /// .. 일회용
    /// </summary>
    [SerializeReferenceDropdownName("일회용"), Substitution("일회용")]
    public sealed class DisposableCard : ITMCardSpecialEffect
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
