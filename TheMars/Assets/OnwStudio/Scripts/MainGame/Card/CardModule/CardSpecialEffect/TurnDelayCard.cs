using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 소요 (턴)
    /// </summary>
    [SerializeReferenceDropdownName("소요 (턴)")]
    public sealed class TurnDelayCard : ITMCardSpecialEffect
    {
        /// <summary>
        /// .. 소요시킬 턴
        /// </summary>
        [field: SerializeField, DisplayAs("소요 턴")] public int DelayTurn { get; private set; } = 5;

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = ()
                => TMCardGameManager.Instance.DelayTurn(cardController, DelayTurn);
        }
    }
}
