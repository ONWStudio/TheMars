using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("드로우"), Substitution("드로우")]
    public sealed class DrawEffectCreator : ITMEffectCreator
    {
        [SerializeReference, DisplayAs("드로우 효과"), FormerlySerializedAs("_drawEffectCreators"), SerializeReferenceDropdown] private List<ITMNormalEffectCreator> _drawEffectCreators = new();

        public IEnumerable<ITMNormalEffect> DrawEffects => _drawEffectCreators
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>(); 

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<DrawEffect, DrawEffectCreator>(this);
        }
    }
}
