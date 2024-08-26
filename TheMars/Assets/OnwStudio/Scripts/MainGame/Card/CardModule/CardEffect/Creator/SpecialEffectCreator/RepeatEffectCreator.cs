using Onw.Attribute;
namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("(특수) 반복"), Substitution("반복")]
    public sealed class RepeatEffectCreator : ITMCardSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<RepeatEffect, RepeatEffectCreator>(this);
        }
    }
}
