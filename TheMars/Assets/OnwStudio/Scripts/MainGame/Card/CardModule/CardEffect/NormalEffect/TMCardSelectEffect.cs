using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 미구현
    /// </summary>
    public sealed class TMCardSelectEffect : ITMNormalEffect
    {
        public string Description => "카드 발견";

        public void ApplyEffect(TMCardModel model, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState => Debug.Log("카드 발견"));
        }
    }
}
