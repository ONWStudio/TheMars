using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 드로우")]
    public sealed class TMCardDrawEffectCreator : ITMNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardDrawEffect, TMCardDrawEffectCreator>(this); 
        }
    }
}
