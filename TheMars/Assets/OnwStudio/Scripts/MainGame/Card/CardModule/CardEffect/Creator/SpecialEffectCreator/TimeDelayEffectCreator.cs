using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("�ҿ� (�ð�)"), Substitution("�ҿ�(�ð�)")]
    public class TimeDelayEffectCreator : ITMEffectCreator
    {
        [field: SerializeField, DisplayAs("������ �ð� (��)")] public float DelayTime { get; private set; } = 1f;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TimeDelayEffect, TimeDelayEffectCreator>(this);
        }
    }
}
