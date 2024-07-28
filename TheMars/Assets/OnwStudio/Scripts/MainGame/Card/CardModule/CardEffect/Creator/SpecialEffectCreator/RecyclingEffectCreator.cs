using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("��Ȱ��"), Substitution("��Ȱ��")]
    public sealed class RecyclingEffectCreator : ITMEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<RecyclingEffect, RecyclingEffectCreator>(this);
        }
    }
}
