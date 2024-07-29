using System.Collections;
using System.Collections.Generic;
using TMCard.Effect.Resource;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.Effect.Resource
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("자원 획득 (테라)")]
    public sealed class TeraResourceEffectCreator : IResourceEffectCreator
    {
        [field: SerializeField, DisplayAs("획득량"), Tooltip("테라")] public int Amount { get; private set; } 

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TeraResourceEffect, TeraResourceEffectCreator>(this);
        }
    }
}
