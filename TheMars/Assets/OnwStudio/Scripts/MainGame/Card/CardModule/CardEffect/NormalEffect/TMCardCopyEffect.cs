using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public sealed class TMCardCopyEffect : ITMNormalEffect, ITMInitializableEffect<TMCardCopyEffectCreator>
    {
        [SerializeField, ReadOnly]
        private TMCardData _copyCardData = null;
        [SerializeField, ReadOnly]
        private int _copyCount = 0;

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() => 
            {
                Debug.Log("카드 카피");
            });
        }

        public void Initialize(TMCardCopyEffectCreator effectCreator)
        {
            _copyCardData = effectCreator.CopyCardData;
            _copyCount = effectCreator.CopyCount;
        }
    }
}