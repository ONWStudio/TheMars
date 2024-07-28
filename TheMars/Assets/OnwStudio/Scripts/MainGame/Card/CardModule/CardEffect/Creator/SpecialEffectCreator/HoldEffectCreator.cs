using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("����"), Substitution("����")]
    public sealed partial class HoldEffectCreator : ITMEffectCreator
    {
        public IEnumerable<ITMNormalEffect> NormalEffects => _holdEffects
            .Select(creator => creator.CreateEffect())
            .OfType<ITMNormalEffect>();

        [field: SerializeField, FormerlySerializedAs("<FriendlyCard>k__BackingField"), DisplayAs("�ߵ� Ʈ���� ī��"), Tooltip("���� ȿ���� �ߵ��Ҷ� ������ ī�� ID")]
        public TMCardData FriendlyCard { get; private set; } = null;

        [SerializeReference, FormerlySerializedAs("_holdEffects"), DisplayAs("���� ȿ��"), Tooltip("���� ȿ��"), SerializeReferenceDropdown]
        private List<ITMNormalEffectCreator> _holdEffects = new();

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<HoldEffect, HoldEffectCreator>(this);
        }
    }
}
