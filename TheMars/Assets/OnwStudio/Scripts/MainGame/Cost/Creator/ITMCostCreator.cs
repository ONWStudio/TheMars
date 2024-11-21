using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Cost.Creator
{
    public interface ITMResourceCostCreator : ITMCostCreator
    {
        TMResourceKind Kind { get; }
        int Cost { get; }
    }

    public interface ITMCostCreator
    {
        public static TCost CreateEffect<TCost, TCreator>(TCreator creator)
            where TCost : ITMCost, new()
            where TCreator : ITMCostCreator
        {
            TCost effect = new();

            if (effect is ITMInitializeCost<TCreator> initializeEffect)
            {
                initializeEffect.Initialize(creator);
            }

            return effect;
        }

        ITMCost CreateCost();
    }
}