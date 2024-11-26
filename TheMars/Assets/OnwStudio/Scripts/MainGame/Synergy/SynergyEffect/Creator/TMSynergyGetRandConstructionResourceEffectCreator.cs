using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Synergy.Effect.Creator
{
    [SerializeReferenceDropdownName("랜덤 건설 자원 획득(강철, 식물, 점토)")]
    public class TMSynergyGetRandConstructionResourceEffectCreator : TMSynergyEffectCreator
    {
        [field: SerializeField, DisplayAs("획득할 건설 자원 개수")] public int Resource { get; private set; } = 10;
        [field: SerializeField, DisplayAs("반복 일수"), OnwMin(1)] public int RepeatDay { get; private set; } = 1;
        
        public override TMSynergyEffect CreateEffect()
        {
            return CreateEffect<TMSynergyGetRandConstructionResourceEffect, TMSynergyGetRandConstructionResourceEffectCreator>(this);
        }
    }
}
