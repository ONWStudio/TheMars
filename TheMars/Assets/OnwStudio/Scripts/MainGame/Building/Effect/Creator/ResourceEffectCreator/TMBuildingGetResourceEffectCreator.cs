using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Building.Effect.Creator
{
    using static ITMBuildingEffectCreator;

    [System.Serializable, SerializeReferenceDropdownName("자원 획득 효과")]
    public sealed class TMBuildingGetResourceEffectCreator : ITMBuildingGetResourceEffectCreator
    {
        [field: SerializeField, DisplayAs("트리거 시간")] public float RepeatSeconds { get; private set; } = 0f;
        [field: SerializeField, DisplayAs("종류")] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, DisplayAs("획득량")] public int AdditionalResource { get; private set; }

        
        public ITMBuildingEffect CreateEffect()
        {
            return CreateEffect<TMBuildingGetResourceEffect, TMBuildingGetResourceEffectCreator>(this);
        }
 
    }
}
