using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("ī�� �߰�")]
    public sealed class TMCardSelectEffectCreator : ITMNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardSelectEffect, TMCardSelectEffectCreator>(this);
        }
    }
}
