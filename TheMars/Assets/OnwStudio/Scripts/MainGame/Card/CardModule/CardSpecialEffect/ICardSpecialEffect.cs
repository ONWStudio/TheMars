namespace TMCard.SpecialEffect
{
    using UI;

    /// <summary>
    /// .. 특수 효과에 따라서 카드의 동작이 바뀌기 때문에 인터페이스를 통해 카드의 실제 구현된 객체에 접근하고 콜백 메서드로 동작을 정의합니다
    /// </summary>
    public interface ITMCardSpecialEffect
    {
        int No { get; }
        void ApplyEffect(TMCardController cardController);
    }
}