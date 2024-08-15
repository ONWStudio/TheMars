using Onw.Attribute;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 반복"), Substitution("반복")]
    public sealed class RepeatEffectCreator : ITMSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<RepeatEffect, RepeatEffectCreator>(this);
        }
    }
}
