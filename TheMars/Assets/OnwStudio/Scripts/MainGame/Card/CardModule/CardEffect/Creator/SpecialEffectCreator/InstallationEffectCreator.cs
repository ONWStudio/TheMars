using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 설치"), Substitution("설치")]
    public sealed class InstallationEffectCreator : ITMSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<InstallationEffect, InstallationEffectCreator>(this);
        }
    }
}
