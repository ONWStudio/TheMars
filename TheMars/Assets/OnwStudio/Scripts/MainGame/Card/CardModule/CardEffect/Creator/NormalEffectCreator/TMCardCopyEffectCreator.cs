using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 고정 획득")]
    public sealed class TMCardCopyEffectCreator : ITMNormalEffectCreator
    {
        [field: SerializeField, FormerlySerializedAs("<CopyCardData>k__BackingField"), DisplayAs("고정 획득 카드")] public TMCardData CopyCardData { get; private set; } = null;
        [field: SerializeField, FormerlySerializedAs("<CopyCount>k__BackingField"), DisplayAs("획득 개수"), Min(1)] public int CopyCount { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardCopyEffect, TMCardCopyEffectCreator>(this);
        }
    }
}
