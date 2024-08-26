using Onw.Attribute;
namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("(특수) 설치"), Substitution("설치")]
    public sealed class InstallationEffectCreator : ITMCardSpecialEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<InstallationEffect, InstallationEffectCreator>(this);
        }
    }
}
