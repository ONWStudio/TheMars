using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Effect;
using AYellowpaper.SerializedCollections;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 일회용"), Substitution("일회용")]
    public sealed class DisposableEffectCreator : ITMSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<DisposableEffect, DisposableEffectCreator>(this);
        }
    }
}
