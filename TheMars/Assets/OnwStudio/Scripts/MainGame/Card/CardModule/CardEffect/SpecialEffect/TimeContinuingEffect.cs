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
    public sealed class TimeContinuingEffect : ITMCardSpecialEffect, ITMInitializableEffect<TimeContinuingEffectCreator>
    {
        /// <summary>
        /// .. 지속 시간
        /// </summary>
        [field: SerializeField, ReadOnly] public float ContinuingTime { get; private set; } = 1f;

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("지속(시간)");

        // .. TODO : 버프 디버프류 이펙트 추가
        public void ApplyEffect(TMCardController cardController)
        {
            //cardController.UseState = () => TMCardGameManager.Instance.OnContinuingSeconds(
            //    cardController,
            //    ContinuingTime,
            //    () => { });
        }

        public void Initialize(TimeContinuingEffectCreator effectCreator)
        {
            ContinuingTime = effectCreator.ContinuingTime;
        }
    }
}