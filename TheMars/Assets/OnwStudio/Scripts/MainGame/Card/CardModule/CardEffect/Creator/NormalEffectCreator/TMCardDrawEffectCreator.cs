using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("ī�� ��ο�")]
    public sealed class TMCardDrawEffectCreator : ITMNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardDrawEffect, TMCardDrawEffectCreator>(this); 
        }
    }
}
