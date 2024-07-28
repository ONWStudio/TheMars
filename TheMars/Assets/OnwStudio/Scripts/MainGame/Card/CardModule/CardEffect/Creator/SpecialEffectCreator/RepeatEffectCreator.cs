using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TMCard.Effect;
using UnityEngine;

namespace TMCard.Efffect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("반복"), Substitution("반복")]
    public sealed class RepeatEffectCreator : ITMEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<RepeatEffect, RepeatEffectCreator>(this);
        }
    }
}
