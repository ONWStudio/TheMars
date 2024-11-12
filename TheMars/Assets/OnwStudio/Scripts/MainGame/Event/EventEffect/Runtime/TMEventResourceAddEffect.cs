using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Event.Effect.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    public class TMEventResourceAddEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventResourceAddEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; } 
        [field: SerializeField, ReadOnly] public TMResourceKind ResourceKind { get; set; }
        [field: SerializeField, ReadOnly] public int Resource { get; set; }
        
        public void Initialize(TMEventResourceAddEffectCreator creator)
        {
            ResourceKind = creator.ResourceKind;
            Resource = creator.Resource;
        }
        
        public void ApplyEffect()
        {
            TMPlayerManager.Instance.AddResource(ResourceKind, Resource);
        }
    }
}
