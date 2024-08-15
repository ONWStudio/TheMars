using Onw.Attribute;
using UnityEngine;
using UnityEngine.Serialization;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 버리기")]
    public sealed class TMCardDropEffectCreator : ITMNormalEffectCreator
    {
        [field: SerializeField, Min(1), DisplayAs("개수"), FormerlySerializedAs("<DropCount>k__BackingField")]
        public int DropCount { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardDropEffect, TMCardDropEffectCreator>(this);
        }
    }
}
