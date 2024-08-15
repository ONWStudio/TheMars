using Onw.Attribute;
namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("(특수) 신기루"), Substitution("신기루")]
    public sealed class MirageEffectCreator : ITMSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<MirageEffect, MirageEffectCreator>(this);
        }
    }
}