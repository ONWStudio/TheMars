using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using Onw.Extensions;
using TM.Event.Effect.Creator;
using TM.Runtime;
using TM.Card.Runtime;
using TM.Card;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventRandCardCollectEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventRandCardCollectEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; }
        [field: SerializeField, ReadOnly] public TMCardKindForWhere Kind { get; private set; }
        [field: SerializeField, ReadOnly] public int CollectCount { get; private set; }

        public void Initialize(TMEventRandCardCollectEffectCreator effectCreator)
        {
            Kind = effectCreator.Kind;
            CollectCount = effectCreator.CollectCount;  
        }

        public void ApplyEffect()
        {
            TMCardNotifyIconSpawner.Instance.CreateIcon(TMCardManager
                .Instance
                .CardCreator
                .CreateRandomCardsByWhere(isTargetCard, CollectCount, false));

            bool isTargetCard(TMCardData cardData)
            {
                return Kind switch
                {
                    TMCardKindForWhere.CONSTRUCTION => cardData.Kind == TMCardKind.CONSTRUCTION,
                    TMCardKindForWhere.EFFECT => cardData.Kind == TMCardKind.EFFECT,
                    TMCardKindForWhere.ALL => true,
                    _ => false
                };
            }
        }
    }
}