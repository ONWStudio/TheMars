using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 카드 획득
    /// </summary>
    public sealed class TMCardCollectEffect : ITMNormalEffect, ITMInitializableEffect<TMCardCollectEffectCreator>
    {
        [SerializeField, ReadOnly] private int _collectCount = 0;

        public void Initialize(TMCardCollectEffectCreator effectCreator)
        {
            _collectCount = effectCreator.CollectCount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() 
                => TMCardGameManager.Instance.CollectCard(controller, _collectCount));
        }
    }
}