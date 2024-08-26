using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect.Resource
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("자원 획득 (마르스 리튬)")]
    public sealed class MarsLithumCardEffectCreator : IResourceCardEffectCreator
    {
        [field: SerializeField, DisplayAs("획득량"), Tooltip("마르스 리튬")] public int Amount { get; private set; }

        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<MarsLithumEffect, MarsLithumCardEffectCreator>(this);
        }
    }
}
