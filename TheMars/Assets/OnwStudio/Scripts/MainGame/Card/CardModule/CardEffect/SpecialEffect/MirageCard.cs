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
    [SerializeReferenceDropdownName("신기루"), Substitution("신기루")]
    public sealed class MirageCard : ITMCardSpecialEffect
    {
        public int No => 2;
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("신기루");

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.DisposeCard(cardController);
            cardController.TurnEndedState = () => TMCardGameManager.Instance.DestroyCard(cardController);
        }
    }
}
