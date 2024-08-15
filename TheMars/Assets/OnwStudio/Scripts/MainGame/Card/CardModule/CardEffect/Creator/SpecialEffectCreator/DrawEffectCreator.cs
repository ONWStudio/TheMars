using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.Serialization;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 드로우")]
    public sealed class DrawEffectCreator : ITMSpecialEffectCreator
    {
        [SerializeReference, DisplayAs("드로우 효과"), FormerlySerializedAs("_drawEffectCreators"), SerializeReferenceDropdown]
        private List<ITMNormalEffectCreator> _drawEffectCreators = new();

        public IEnumerable<ITMNormalEffect> DrawEffects => _drawEffectCreators
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>();

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<DrawEffect, DrawEffectCreator>(this);
        }
    }
}
