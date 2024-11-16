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
        [SerializeField] private LocalizedString _effectDescription = new("TM_Event_Effect", "Resource_Add_Effect");

        public override LocalizedString EffectDescription => _effectDescription;
        [field: SerializeField] public int Resource { get; set; }
        
        public void Initialize(TMEventResourceAddEffectCreator creator)
        {
            base.Initialize(creator);
            Resource = creator.Resource;

            _effectDescription.Arguments = new object[]
            {
                new
                {
                    Kind = Kind.ToString(),
                    Resource = Mathf.Abs(Resource),
                    Positive = Resource >= 0
                }
            };
        }
        
        public override void ApplyEffect()
        {
            TMPlayerManager.Instance.AddResource(Kind, Resource);
        }
    }
}
