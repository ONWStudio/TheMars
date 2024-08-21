using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 미구현
    /// </summary>
    public sealed class TMCardRemoveEffect : ITMNormalEffect
    {
        public string Description => $"덱, 패, 무덤에서 카드를 제거";
        
        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            Debug.Log("카드 제거");
        }
    }
}