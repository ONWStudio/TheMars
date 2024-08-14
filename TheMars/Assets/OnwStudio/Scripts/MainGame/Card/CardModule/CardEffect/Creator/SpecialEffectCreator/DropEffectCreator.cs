using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 버리기"), Substitution("버리기")]
    public sealed class DropEffectCreator : ITMSpecialEffectCreator
    {
        [SerializeReference, DisplayAs("버리기"), FormerlySerializedAs("_cardEffectCreators"), SerializeReferenceDropdown]
        private List<ITMNormalEffectCreator> _cardEffectCreators = new();

        public IEnumerable<ITMNormalEffect> DropEffects => _cardEffectCreators
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>();

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<DropEffect, DropEffectCreator>(this);
        }
    }
}
