using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 소요 (시간)
    public sealed class TimeDelayEffect : ITMCardSpecialEffect, ITMInitializableEffect<TimeDelayEffectCreator>
    {
        /// <summary>
        /// .. 딜레이 타임
        /// </summary>
        [field: SerializeField, DisplayAs("딜레이 시간 (초)"), ReadOnly] public float DelayTime { get; private set; } = 1f;

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("소요(시간)");

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = () => TMCardGameManager.Instance.DelaySeconds(cardController, DelayTime);
        }

        public void Initialize(TimeDelayEffectCreator effectCreator)
        {
            DelayTime = effectCreator.DelayTime;
        }
    }
}
