using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 소요 (시간)
    /// </summary>
    [SerializeReferenceDropdownName("소요 (시간)"), Substitution("소요(시간)")]
    public sealed class TimeDelayCard : ITMCardSpecialEffect
    {
        /// <summary>
        /// .. 딜레이 타임
        /// </summary>
        [field: SerializeField, DisplayAs("딜레이 시간 (초)")] public float DelayTime { get; private set; } = 1f;

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("소요(시간)");

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.DelaySeconds(cardController, DelayTime);
        }
    }
}
