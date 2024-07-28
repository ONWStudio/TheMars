using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Interface;
using TMCard.Runtime;
using TMCard.Effect;
using TMCard.Effect.Resource;
using TMCard;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 반복
    /// </summary>
    public sealed class RepeatEffect : ITMCardSpecialEffect, IDescriptable, ILabel
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("반복");
        public string Description => "";

        private sealed class EffectInitialAmountPair
        {
            public int UseCount { get; set; }
            public IReadOnlyList<ITMCardResourceEffect> ResourceEffects { get; }
            public IReadOnlyList<ITMNormalEffect> NormalEffects { get; }

            public EffectInitialAmountPair(List<ITMCardResourceEffect> resourceEffects, List<ITMNormalEffect> normalEffects)
            {
                UseCount = 0;
                ResourceEffects = resourceEffects;
                NormalEffects = normalEffects;
            }
        }

        private static readonly Dictionary<string, EffectInitialAmountPair> _resourceEffectInitalAmounts = new();

        public void ApplyEffect(TMCardController cardController)
        {
            if (!_resourceEffectInitalAmounts.TryGetValue(cardController.CardData.Guid, out EffectInitialAmountPair effectInitialAmountPair))
            {
                List<ITMNormalEffect> normalEffects = new();
                List<ITMCardResourceEffect> resourceEffects = new();

                foreach (ITMNormalEffect cardEffect in cardController.CardData.GetEffectOfType<ITMNormalEffect>())
                {
                    if (cardEffect is ITMCardResourceEffect resourceEffect)
                    {
                        resourceEffects.Add(resourceEffect);
                    }
                    else
                    {
                        normalEffects.Add(cardEffect);
                    }
                }

                effectInitialAmountPair = new(resourceEffects, normalEffects);
                _resourceEffectInitalAmounts.Add(cardController.CardData.Guid, effectInitialAmountPair);
            }

            cardController.UseState = () =>
            {
                TMCardGameManager.Instance.EffectCard(cardController);

                effectInitialAmountPair
                    .NormalEffects
                    .ForEach(cardEffect => cardEffect.ApplyEffect(cardController));

                effectInitialAmountPair
                    .ResourceEffects
                    .ForEach(cardEffect => cardEffect.OnResourceEffect(cardController, cardEffect.Amount * effectInitialAmountPair.UseCount));

                effectInitialAmountPair.UseCount++;
            };
        }
    }
}