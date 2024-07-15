using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 일회용
    /// </summary>
    [SerializeReferenceDropdownName("일회용")]
    public sealed class DisposableCard : ITMCardSpecialEffect
    {
        public int No => 0;

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () =>
            {
                cardController.CardData.UseCard();
                TMCardGameManager.Instance.DisposeCard(cardController);
            };
        }
    }
}
