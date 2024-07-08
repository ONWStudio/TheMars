using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 지속 (시간)
    /// </summary>
    [SerializeReferenceDropdownName("지속 (시간)")]
    public sealed class TimeContinuingCard : ITMCardSpecialEffect
    {
        /// <summary>
        /// .. 지속 시간
        /// </summary>
        [field: SerializeField, DisplayAs("지속 시간")] public float ContinuingTime { get; private set; } = 1f;

        // .. TODO : 버프 디버프류 이펙트 추가

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.OnContinuingSeconds(
                cardController,
                ContinuingTime,
                () => { });
        }
    }
}