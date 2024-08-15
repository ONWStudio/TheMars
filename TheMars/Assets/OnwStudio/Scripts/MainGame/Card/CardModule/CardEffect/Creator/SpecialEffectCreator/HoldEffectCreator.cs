using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.Serialization;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 보유"), Substitution("보유")]
    public sealed partial class HoldEffectCreator : ITMSpecialEffectCreator
    {
        public IEnumerable<ITMNormalEffect> NormalEffects => _holdEffects
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>();

        [field: SerializeField, FormerlySerializedAs("<FriendlyCard>k__BackingField"), DisplayAs("트리거 카드"), Tooltip("보유 효과의 트리거가 될 카드")]
        public TMCardData FriendlyCard { get; private set; }

        [SerializeReference, FormerlySerializedAs("_holdEffects"), DisplayAs("보유 효과"), Tooltip("보유 효과"), SerializeReferenceDropdown]
        private List<ITMNormalEffectCreator> _holdEffects = new();

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<HoldEffect, HoldEffectCreator>(this);
        }
    }
}
