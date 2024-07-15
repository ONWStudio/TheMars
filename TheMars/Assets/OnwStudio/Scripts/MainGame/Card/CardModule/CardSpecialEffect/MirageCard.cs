using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 신기루
    /// </summary>
    [SerializeReferenceDropdownName("신기루")]
    public sealed class MirageCard : ITMCardSpecialEffect
    {
        public int No => 2;

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.DisposeCard(cardController);
            cardController.TurnEndedState = () => TMCardGameManager.Instance.DestroyCard(cardController);
        }
    }
}
