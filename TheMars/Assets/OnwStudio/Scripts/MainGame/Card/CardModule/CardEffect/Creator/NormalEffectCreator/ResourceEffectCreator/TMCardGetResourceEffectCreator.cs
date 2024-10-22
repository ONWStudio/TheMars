using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TM.Class;

namespace TM.Card.Effect.Creator
{
    using static TMCardEffectCreator;

    [SerializeReferenceDropdownName("자원 획득")]
    public sealed class TMCardGetResourceEffectCreator : TMCardNormalEffectCreator
    {
        [field: SerializeField]
        public IReadOnlyList<TMResourceData> Resources => _resources;


        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once CollectionNeverUpdated.Local
        [SerializeField, DisplayAs("자원 획득")] private List<TMResourceData> _resources = new();
        
        public override ITMCardEffect CreateEffect()
        {
            return CreateEffect<TMCardGetResourceEffect, TMCardGetResourceEffectCreator>(this);
        }
    }
}
