using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Effect;
using AYellowpaper.SerializedCollections;
using Onw.Attribute;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("��ȸ��"), Substitution("��ȸ��")]
    public sealed class DisposableEffectCreator : ITMEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<DisposableEffect, DisposableEffectCreator>(this);
        }
    }
}
