using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Class;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    using static ITMBuildingEffectCreator;
    
    public sealed class TMBuildingGetResourceEffectCreator : ITMBuildingGetResourceEffectCreator
    {
        public IReadOnlyList<TMResourceData> Resources => _resources;

        [field: SerializeField] public float RepeatSeconds { get; private set; } = 0f;
        
        [SerializeField, DisplayAs("자원 획득")] private List<TMResourceData> _resources = new();
        
        public ITMBuildingEffect CreateEffect()
        {
            return BuildingEffectGenerator.CreateEffect<TMBuildingGetResourceEffect, TMBuildingGetResourceEffectCreator>(this);
        }
 
    }
}
