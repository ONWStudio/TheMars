using Onw.Interface;
using TMCard.Runtime;
namespace TMCard.Effect
{

    
    public interface ITMCardEffect : IDescriptable
    {
        /// <summary>
        /// .. 카드 사용시
        /// </summary>
        void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger);
    }
}