using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TM.Card.Effect.Creator
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("카드 획득")]
    public sealed class TMCardCollectEffectCreator : ITMCardNormalEffectCreator
    {
        [field: SerializeField, Range(1, 4), FormerlySerializedAs("<CollecCount>k__BackingField"), DisplayAs("선택 카드 개수")]
        public int CollectCount { get; private set; } = 1;

        [field: SerializeField, FormerlySerializedAs("<SelectKind>k__BackingField"), DisplayAs("카드군")]
        public TMCardKind SelectKind { get; private set; } = TMCardKind.NONE;

        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<TMCardCollectEffect, TMCardCollectEffectCreator>(this);
        }
    }
}
