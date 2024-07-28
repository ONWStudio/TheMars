using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("신기루"), Substitution("신기루")]
    public sealed class MirageEffectCreator : ITMEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<MirageEffect, MirageEffectCreator>(this);
        }
    }
}