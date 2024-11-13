using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Event.Effect.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventResourceAddEffect : TMEventResourceAddEffectBase, ITMEventInitializeEffect<TMEventResourceAddEffectCreator>
    {
        [SerializeField] private LocalizedString _effectDescription = new();

        public override LocalizedString EffectDescription => _effectDescription;
        [field: SerializeField] public TMResourceKind ResourceKind { get; set; }
        [field: SerializeField] public int Resource { get; set; }
        
        public void Initialize(TMEventResourceAddEffectCreator creator)
        {
            base.Initialize(creator);
            Resource = creator.Resource;
        }
        
        public override void ApplyEffect()
        {
            TMPlayerManager.Instance.AddResource(ResourceKind, Resource);
        }
    }
}
