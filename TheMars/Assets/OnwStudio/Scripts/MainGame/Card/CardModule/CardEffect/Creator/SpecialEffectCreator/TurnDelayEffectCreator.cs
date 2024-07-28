using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("�ҿ� (��)"), Substitution("�ҿ�(��)")]
    public sealed class TurnDelayEffectCreator : ITMEffectCreator
    {
        [field: SerializeField, DisplayAs("�ҿ� ��")] public int DelayTurn { get; private set; } = 5;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TurnDelayEffect, TurnDelayEffectCreator>(this);
        }
    }
}
