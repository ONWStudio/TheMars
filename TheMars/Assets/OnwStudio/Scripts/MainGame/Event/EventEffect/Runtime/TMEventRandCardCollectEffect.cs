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
using System;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventRandCardCollectEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventRandCardCollectEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; } = new("TM_Event_Effect", "Rand_Card_Collect_Effect");
        [field: SerializeField, ReadOnly] public TMCardKindForWhere Kind { get; private set; }
        [field: SerializeField, ReadOnly] public int CollectCount { get; private set; }

        public void Initialize(TMEventRandCardCollectEffectCreator creator)
        {
            Kind = creator.Kind;
            CollectCount = creator.CollectCount;
            EffectDescription.Arguments = new object[]
            {
                new
                {
                    Kind = Kind.ToString(),
                    CollectCount
                }
            };
        }

        public void ApplyEffect()
        {
            TMCardModel[] cardArray = TMCardManager
                .Instance
                .CardCreator
                .CreateRandomCardsByWhere(isTargetCard, CollectCount, false);

            if (cardArray.Length > 0)
            {
                TMCardNotifyIconSpawner.Instance.CreateIcon(cardArray);
            }

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