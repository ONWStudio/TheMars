using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect.Resource
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("자원 획득 (마르스 리튬)")]
    public sealed class MarsLithumEffectCreator : IResourceEffectCreator
    {
        [field: SerializeField, DisplayAs("획득량"), Tooltip("마르스 리튬")] public int Amount { get; private set; }

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<MarsLithumEffect, MarsLithumEffectCreator>(this);
        }
    }
}
