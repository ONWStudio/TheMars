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
    public sealed class RepeatEffect : TMCardSpecialEffect
    {
        public readonly struct ResourcePair
        {
            public ITMCardResourceEffect ResourceEffect { get; }
            public int OriginalAmount { get; }

            public ResourcePair(ITMCardResourceEffect resourceEffect, int originalAmount)
            {
                ResourceEffect = resourceEffect;
                OriginalAmount = originalAmount;
            }
        }

        public readonly List<ResourcePair> ResourcePairs = new(); 

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
           ResourcePairs.AddRange(controller
                .Effects
                .OfType<ITMCardResourceEffect>()
                .Select(effect => new ResourcePair(effect, effect.Amount)));

            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();

                ResourcePairs
                    .ForEach(resourcePair => resourcePair.ResourceEffect.AddRewardResource(resourcePair.OriginalAmount));

                TMCardHelper.Instance.MoveToScreenCenterAfterToTomb(controller);
            });
        }

        public RepeatEffect() : base("Repeat") {}
    }
}