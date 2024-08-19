using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class TMCardSelectEffect : ITMNormalEffect
    {
        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState => Debug.Log("카드 발견"));
        }
    }
}
