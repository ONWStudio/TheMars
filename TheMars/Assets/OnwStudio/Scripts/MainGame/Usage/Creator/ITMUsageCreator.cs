using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Usage.Creator
{
    public interface ITMUsageCreator
    {
        public static TEffect CreateEffect<TEffect, TCreator>(TCreator creator)
            where TEffect : ITMUsage, new()
            where TCreator : ITMUsageCreator
        {
            TEffect effect = new();

            if (effect is ITMInitializeUsage<TCreator> initializeEffect)
            {
                initializeEffect.Initialize(creator);
            }

            return effect;
        }

        ITMUsage CreateUsage();
    }
}