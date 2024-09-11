using UnityEngine.Events;
using TM.Card.Runtime;

namespace TM.Card.Effect
{
    public interface ITMCardEffectTrigger
    {
        event UnityAction<TMCardModel> OnEffectEvent;
    }
}