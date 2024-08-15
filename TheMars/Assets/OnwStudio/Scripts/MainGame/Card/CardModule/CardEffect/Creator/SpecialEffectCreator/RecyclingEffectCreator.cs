using Onw.Attribute;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 재활용"), Substitution("재활용")]
    public sealed class RecyclingEffectCreator : ITMSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<RecyclingEffect, RecyclingEffectCreator>(this);
        }
    }
}
