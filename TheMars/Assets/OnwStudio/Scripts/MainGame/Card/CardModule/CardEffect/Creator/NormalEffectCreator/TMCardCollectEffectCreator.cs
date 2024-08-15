using Onw.Attribute;
using UnityEngine;
using UnityEngine.Serialization;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 획득")]
    public sealed class TMCardCollectEffectCreator : ITMNormalEffectCreator
    {
        [field: SerializeField, Range(1, 4), FormerlySerializedAs("<CollecCount>k__BackingField"), DisplayAs("선택 카드 개수")]
        public int CollectCount { get; private set; } = 1;

        [field: SerializeField, FormerlySerializedAs("<SelectKind>k__BackingField"), DisplayAs("카드군")]
        public TMCardKind SelectKind { get; private set; } = TMCardKind.NONE;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardCollectEffect, TMCardCollectEffectCreator>(this);
        }
    }
}
