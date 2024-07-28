using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("���� (��)"), Substitution("����(��)")]
    public class TurnContinuingEffectCreator : ITMEffectCreator
    {
        [field: SerializeField, DisplayAs("���� ��")] public int ContinuingTurn { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TurnContinuingEffect, TurnContinuingEffectCreator>(this);
        }
    }
}