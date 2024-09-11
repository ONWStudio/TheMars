using Onw.Interface;
using TM.Card.Runtime;

namespace TM.Card.Effect
{
    public interface ITMCardEffect : IDescriptable
    {
        /// <summary>
        /// .. 카드 사용시
        /// </summary>
        void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger);
    }
}