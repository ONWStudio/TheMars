using Onw.Interface;
namespace TMCard.Effect.Resource
{
    public interface ITMCardResourceEffect : ITMNormalEffect, IAmountHolder, INotifier
    {
        /// <summary>
        /// .. 외부에서 TMCardResourceEffect에 직접 접근하여 쓰는 경우가 있을때 사용하는 메서드 입니다
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="additionalAmount"></param>
        void AddRewardResource(int additionalAmount);
    }
}