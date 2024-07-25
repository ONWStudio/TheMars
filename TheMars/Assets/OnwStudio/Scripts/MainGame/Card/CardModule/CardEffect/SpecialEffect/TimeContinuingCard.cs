using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 지속 (시간)
    /// </summary>
    [SerializeReferenceDropdownName("지속 (시간)"), Substitution("지속(시간)")]
    public sealed class TimeContinuingCard : ITMCardSpecialEffect
    {
        /// <summary>
        /// .. 지속 시간
        /// </summary>
        [field: SerializeField, DisplayAs("지속 시간")] public float ContinuingTime { get; private set; } = 1f;

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("지속(시간)");

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