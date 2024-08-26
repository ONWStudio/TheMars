namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("카드 발견")]
    public sealed class ItmCardCardSelectEffectCreator : ITMCardNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<TMCardSelectEffect, ItmCardCardSelectEffectCreator>(this);
        }
    }
}
