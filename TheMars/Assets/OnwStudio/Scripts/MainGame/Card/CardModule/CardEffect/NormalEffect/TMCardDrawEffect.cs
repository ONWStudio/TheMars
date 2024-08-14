using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    // .. TODO : 드로우 효과로 드로우 카드 획득 시 두장 겹쳐서 나오는 버그, 중간중간 알 수 없는 이유로 화면 중앙으로 이동하지 않고 바로 패로 이동하는 
    public sealed class TMCardDrawEffect : ITMNormalEffect, ITMInitializableEffect<TMCardDrawEffectCreator>
    {
        [SerializeField, ReadOnly]
        private int _drawCount = 0;

        public void Initialize(TMCardDrawEffectCreator effectCreator)
        {
            _drawCount = effectCreator.DrawCount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(()
                => controller.DrawCardFromDeck(_drawCount));
        }
    }
}