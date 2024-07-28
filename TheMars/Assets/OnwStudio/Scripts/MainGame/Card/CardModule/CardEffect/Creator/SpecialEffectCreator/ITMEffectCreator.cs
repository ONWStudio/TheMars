using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    public interface ITMNormalEffectCreator : ITMEffectCreator {}

    public interface ITMEffectCreator
    {
        protected static class EffectGenerator
        {
            public static Effect CreateEffect<Effect, Creator>(Creator creator) where Effect : ITMCardEffect, new() where Creator : ITMEffectCreator
            {
                Effect effect = new();

                if (effect is ITMInitializableEffect<Creator> initializeableEffect)
                {
                    initializeableEffect.Initialize(creator);
                }

                return effect;
            }
        }

        ITMCardEffect CreateEffect();
    }
}