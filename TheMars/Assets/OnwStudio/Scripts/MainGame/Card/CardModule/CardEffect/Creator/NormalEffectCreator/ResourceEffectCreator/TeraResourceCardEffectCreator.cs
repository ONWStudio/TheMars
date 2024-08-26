using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect.Resource
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("자원 획득 (테라)")]
    public sealed class TeraResourceCardEffectCreator : IResourceCardEffectCreator
    {
        [field: SerializeField, DisplayAs("획득량"), Tooltip("테라")] public int Amount { get; private set; } 

        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<TeraResourceEffect, TeraResourceCardEffectCreator>(this);
        }
    }
}
