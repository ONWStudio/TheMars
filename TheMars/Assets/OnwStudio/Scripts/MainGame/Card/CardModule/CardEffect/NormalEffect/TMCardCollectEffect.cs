using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. TODO : 카드 획득 효과 수정 카드 선택해서 버리기 
    /// </summary>
    public sealed class TMCardCollectEffect : ITMNormalEffect, ITMInitializableEffect<TMCardCollectEffectCreator>
    {
        [SerializeField, ReadOnly]
        private int _collectCount = 0;
        [SerializeField, ReadOnly]
        private TMCardKind _selectKind = TMCardKind.NONE;

        public void Initialize(TMCardCollectEffectCreator effectCreator)
        {
            _collectCount = effectCreator.CollectCount;
            _selectKind = effectCreator.SelectKind;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() 
                => TMCardHelper.Instance.CollectCard(controller, _collectCount));
        }
    }
}