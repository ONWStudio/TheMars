using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("보유"), Substitution("보유")]
    public sealed partial class HoldEffectCreator : ITMEffectCreator
    {
        public IEnumerable<ITMNormalEffect> NormalEffects => _holdEffects
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>();

        [field: SerializeField, FormerlySerializedAs("<FriendlyCard>k__BackingField"), DisplayAs("발동 트리거 카드"), Tooltip("보유 효과가 발동할때 참조할 카드 ID")]
        public TMCardData FriendlyCard { get; private set; } = null;

        [SerializeReference, FormerlySerializedAs("_holdEffects"), DisplayAs("보유 효과"), Tooltip("보유 효과"), SerializeReferenceDropdown]
        private List<ITMNormalEffectCreator> _holdEffects = new();

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<HoldEffect, HoldEffectCreator>(this);
        }
    }
}
