using System.Collections;
using System.Collections.Generic;
using TM.Building.Effect;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    public interface ITMEventEffectCreator
    {
        protected static class EventEffectGenerator
        {
            public static TEffect CreateEffect<TEffect, TCreator>(TCreator creator)
                where TEffect : ITMEventEffect, new()
                where TCreator : ITMEventEffectCreator
            {
                TEffect effect = new();

                if (effect is ITMEventInitializeEffect<TCreator> initializeEffect)
                {
                    initializeEffect.Initialize(creator);
                }

                return effect;
            }
        }

        ITMEventEffect CreateEffect();
    }
}
