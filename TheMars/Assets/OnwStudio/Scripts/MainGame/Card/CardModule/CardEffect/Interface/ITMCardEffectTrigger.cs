using Onw.Event;
using TMCard.Runtime;
using UnityEngine.Events;
namespace TMCard.Effect
{
    public interface ITMCardEffectTrigger
    {
        event UnityAction<TMCardModel> OnEffectEvent;
    }
}