using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Extensions;

namespace TMCard.SpecialEffect
{
    using UI;
    using Effect;
    using Effect.Resource;

    /// <summary>
    /// .. 반복
    /// </summary>
    [SerializeReferenceDropdownName("반복")]
    public sealed class RepeatCard : ITMCardSpecialEffect
    {
        private sealed class EffectInitialAmountPair
        {
            public int UseCount { get; set; }
            public IReadOnlyList<TMCardResourceEffect> ResourceEffects { get; }
            public IReadOnlyList<ITMCardEffect> NormalEffects { get; }

            public EffectInitialAmountPair(List<TMCardResourceEffect> resourceEffects, List<ITMCardEffect> normalEffects)
            {
                UseCount = 0;
                ResourceEffects = resourceEffects;
                NormalEffects = normalEffects;
            }
        }

        public int No => 9;

        private static readonly Dictionary<string, EffectInitialAmountPair> _resourceEffectInitalAmounts = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void initialize()
        {
            _resourceEffectInitalAmounts.Clear();
        }

        public void ApplyEffect(TMCardController cardController)
        {
            if (!_resourceEffectInitalAmounts.TryGetValue(cardController.CardData.Guid, out EffectInitialAmountPair effectInitialAmountPair))
            {
                List<ITMCardEffect> normalEffects = new();
                List<TMCardResourceEffect> resourceEffects = new();

                foreach (ITMCardEffect cardEffect in cardController.CardData.GetEffectOfType<ITMCardEffect>())
                {
                    if (cardEffect is TMCardResourceEffect resourceEffect)
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
                    .ForEach(cardEffect => cardEffect.OnEffect(cardController.CardData));

                effectInitialAmountPair
                    .ResourceEffects
                    .ForEach(cardEffect => cardEffect.OnResourceEffect(cardController.CardData, cardEffect.Amount * effectInitialAmountPair.UseCount));

                effectInitialAmountPair.UseCount++;
            };
        }
    }
}