using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 지속 (턴)"), Substitution("지속(턴)")]
    public class TurnContinuingEffectCreator : ITMSpecialEffectCreator
    {
        [field: SerializeField, DisplayAs("지속시킬 턴")] public int ContinuingTurn { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TurnContinuingEffect, TurnContinuingEffectCreator>(this);
        }
    }
}