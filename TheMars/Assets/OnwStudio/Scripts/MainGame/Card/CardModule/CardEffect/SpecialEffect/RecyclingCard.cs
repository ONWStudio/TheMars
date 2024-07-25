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
    [SerializeReferenceDropdownName("재활용"), Substitution("재활용")]
    public sealed class RecyclingCard : ITMCardSpecialEffect
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("재활용");

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.RecycleToHand(cardController);
        }
    }
}
