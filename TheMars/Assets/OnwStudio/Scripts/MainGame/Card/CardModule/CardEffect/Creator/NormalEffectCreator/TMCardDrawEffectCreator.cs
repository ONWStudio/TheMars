using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 드로우")]
    public sealed class TMCardDrawEffectCreator : ITMNormalEffectCreator
    {
        [field: SerializeField, Min(1), DisplayAs("드로우 개수")] public int DrawCount { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardDrawEffect, TMCardDrawEffectCreator>(this); 
        }
    }
}
