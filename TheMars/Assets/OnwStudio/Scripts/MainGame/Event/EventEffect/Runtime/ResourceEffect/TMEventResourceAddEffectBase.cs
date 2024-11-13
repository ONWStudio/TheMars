using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    [System.Serializable]
    public abstract class TMEventResourceAddEffectBase : ITMEventEffect, ITMEventInitializeEffect<TMEventResourceAddEffectBaseCreator>
    {
        public abstract LocalizedString EffectDescription { get; }
        
        [field: SerializeField] public TMResourceKind Kind { get; set; }

        public void Initialize(TMEventResourceAddEffectBaseCreator creator)
        {
            Kind = creator.ResourceKind;
        }

        public abstract void ApplyEffect();
    }
}
