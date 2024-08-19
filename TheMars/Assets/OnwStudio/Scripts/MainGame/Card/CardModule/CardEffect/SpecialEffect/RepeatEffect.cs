using System.Collections.Generic;
using System.Linq;
using TMCard.Effect.Resource;
using TMCard.Runtime;
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

            controller.OnClickEvent.RemoveAllToAddListener(eventState =>
            {
                trigger.OnEffectEvent.Invoke(eventState);

                ResourcePairs
                    .ForEach(resourcePair => resourcePair.ResourceEffect.AddRewardResource(resourcePair.OriginalAmount));

                controller.MoveToScreenCenterAfterToTomb();
            });
        }

        public RepeatEffect() : base("Repeat") {}
    }
}