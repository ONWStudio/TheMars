namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("카드 발견")]
    public sealed class TMCardSelectEffectCreator : ITMCardNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<TMCardSelectEffect, TMCardSelectEffectCreator>(this);
        }
    }
}
