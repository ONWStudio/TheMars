using Onw.Attribute;
using UnityEngine;
using UnityEngine.Serialization;
namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("카드 버리기")]
    public sealed class ItmCardCardDropEffectCreator : ITMCardNormalEffectCreator
    {
        [field: SerializeField, Min(1), DisplayAs("개수"), FormerlySerializedAs("<DropCount>k__BackingField")]
        public int DropCount { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<ItmCardCardDropEffect, ItmCardCardDropEffectCreator>(this);
        }
    }
}
