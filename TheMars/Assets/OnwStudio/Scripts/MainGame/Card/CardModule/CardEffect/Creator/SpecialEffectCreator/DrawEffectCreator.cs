using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("��ο�"), Substitution("��ο�")]
    public sealed class DrawEffectCreator : ITMEffectCreator
    {
        [SerializeReference, DisplayAs("��ο� ȿ��"), FormerlySerializedAs("_drawEffectCreators"), SerializeReferenceDropdown] private List<ITMNormalEffectCreator> _drawEffectCreators = new();

        public IEnumerable<ITMNormalEffect> DrawEffects => _drawEffectCreators
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>(); 

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<DrawEffect, DrawEffectCreator>(this);
        }
    }
}
