using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    public interface ITMBuildingEffectCreator
    {
        protected static class BuildingEffectGenerator
        {
            public static TEffect CreateEffect<TEffect, TCreator>(TCreator creator) where TEffect : ITMBuildingEffect, new() where TCreator : ITMBuildingEffectCreator
            {
                TEffect effect = new();

                if (effect is ITMBuildingInitializeEffect<TCreator> initializeEffect)
                {
                    initializeEffect.Initialize(creator);
                }

                return effect;
            }
        }

        ITMBuildingEffect CreateEffect();
    }
}
