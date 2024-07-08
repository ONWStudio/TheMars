using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 재활용
    /// </summary>
    [SerializeReferenceDropdownName("재활용")]
    public sealed class RecyclingCard : ITMCardSpecialEffect
    {
        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.RecycleToHand(cardController);
        }
    }
}
