using Onw.Attribute;
namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("(특수) 신기루"), Substitution("신기루")]
    public sealed class MirageEffectCreator : ITMCardSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<MirageEffect, MirageEffectCreator>(this);
        }
    }
}