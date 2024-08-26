namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("카드 제거")]
    public sealed class ItmCardCardRemoveEffectCreator : ITMCardNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<TMCardRemoveEffect, ItmCardCardRemoveEffectCreator>(this);
        }
    }
}
