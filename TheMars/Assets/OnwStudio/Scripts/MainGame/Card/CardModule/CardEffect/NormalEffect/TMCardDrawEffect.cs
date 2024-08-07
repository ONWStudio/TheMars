using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 추후 반복 효과 적용 가능 시 인터페이스 통합
    /// </summary>
    public sealed class TMCardDrawEffect : ITMNormalEffect, ITMInitializableEffect<TMCardDrawEffectCreator>
    {
        [SerializeField, ReadOnly] private int _drawCount = 0;

        public void Initialize(TMCardDrawEffectCreator effectCreator)
        {
            _drawCount = effectCreator.DrawCount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() 
                => TMCardGameManager.Instance.DrawCardFromDeck(controller, _drawCount));
        }
    }
}