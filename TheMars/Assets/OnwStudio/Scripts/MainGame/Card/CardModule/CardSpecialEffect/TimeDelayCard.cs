using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 소요 (시간)
    /// </summary>
    [SerializeReferenceDropdownName("소요 (시간)")]
    public sealed class TimeDelayCard : ICardSpecialEffect
    {
        /// <summary>
        /// .. 딜레이 타임
        /// </summary>
        [field: SerializeField, DisplayAs("딜레이 시간 (초)")] public float DelayTime { get; private set; } = 1f;

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.DelaySeconds(cardController, DelayTime);
        }
    }
}
