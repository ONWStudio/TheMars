using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Building;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect
{
    public sealed class TMCardBuildingCreateEffectCreator : ITMCardNormalEffectCreator
    {
        [field: SerializeField, DisplayAs("설치 건물")] public TMBuildingData BuildingData { get; private set; } = null;
        
        public ITMCardEffect CreateEffect()
        {
            ITMCardEffectCreator.CardEffectGenerator.CreateEffect<TMCardBuildingCreateEffect, TMCardBuildingCreateEffectCreator>(this);
        }
    }
}