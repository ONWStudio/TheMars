using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("���� (�ð�)"), Substitution("����(�ð�)")]
    public class TimeContinuingEffectCreator : ITMEffectCreator
    {
        [field: SerializeField, DisplayAs("���� �ð�")] public float ContinuingTime { get; private set; } = 1f;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TimeContinuingEffect, TimeContinuingEffectCreator>(this);
        }
    }
}
