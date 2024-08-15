using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 소요 (턴)"), Substitution("소요(턴)")]
    public sealed class TurnDelayEffectCreator : ITMSpecialEffectCreator
    {
        [field: SerializeField, DisplayAs("소요 턴")] public int DelayTurn { get; private set; } = 5;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TurnDelayEffect, TurnDelayEffectCreator>(this);
        }
    }
}
