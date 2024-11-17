using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Card;
using TM.Card.Runtime;
using TM.Event.Effect.Creator;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventRandCardDropEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventRandCardDropEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; } = new("TM_Event_Effect", "Rand_Card_Drop_Effect");
        [field: SerializeField, ReadOnly] public TMCardKindForWhere Kind { get; private set; }
        [field: SerializeField, ReadOnly] public int DropCount { get; private set; }

        public void Initialize(TMEventRandCardDropEffectCreator creator)
        {
            Kind = creator.Kind;
            DropCount = creator.DropCount;
            EffectDescription.Arguments = new object[]
            {
                new
                {
                    Kind,
                    DropCount
                }
            };
        }

        public void ApplyEffect()
        {
            TMCardModel[] dropCards = TMCardManager
                .Instance
                .Cards
                .Where(card => IsTargetCard(card.CardData.Value))
                .OrderBy(_ => Random.value)
                .Take(DropCount)
                .ToArray();

            foreach (TMCardModel card in dropCards)
            {
                TMCardManager.Instance.RemoveCard(card);
                Object.Destroy(card.gameObject);
            }

            bool IsTargetCard(TMCardData cardData)
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
