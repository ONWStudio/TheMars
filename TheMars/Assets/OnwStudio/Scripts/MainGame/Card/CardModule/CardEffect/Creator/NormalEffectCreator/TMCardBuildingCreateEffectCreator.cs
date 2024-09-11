using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TM.Building;

namespace TM.Card.Effect.Creator
{
    using static ITMCardEffectCreator;
    
    [SerializeReferenceDropdownName("건물 설치")]
    public sealed class TMCardBuildingCreateEffectCreator : ITMCardNormalEffectCreator
    {
        [field: SerializeField, DisplayAs("설치 건물")] public TMBuildingData BuildingData { get; private set; } = null;
        
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator
                .CreateEffect<TMCardBuildingCreateEffect, TMCardBuildingCreateEffectCreator>(this);
        }
    }
}