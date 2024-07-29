using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TMCard.Effect;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 반복"), Substitution("반복")]
    public sealed class RepeatEffectCreator : ITMSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<RepeatEffect, RepeatEffectCreator>(this);
        }
    }
}
