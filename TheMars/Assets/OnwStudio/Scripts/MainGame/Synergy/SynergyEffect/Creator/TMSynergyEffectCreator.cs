using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Synergy.Effect.Creator
{
    [System.Serializable]
    public abstract class TMSynergyEffectCreator
    {
        [field: SerializeField, DisplayAs("시너지 건물 개수")] public int TargetBuildingCount { get; private set; }

        protected TEffect CreateEffect<TEffect, TCreator>(TCreator creator)
            where TEffect : TMSynergyEffect, new()
            where TCreator : TMSynergyEffectCreator
        {
            TEffect effect = new() { Creator = this };

            if (effect is ITMSynergyInitializeEffect<TCreator> initializeEffect)
            {
                initializeEffect.Initialize(creator);
            }

            return effect;
        }

        public abstract TMSynergyEffect CreateEffect();
    }
}
