using Onw.Event;
using TMCard.Runtime;
namespace TMCard.Effect
{
    public interface ITMCardEffectTrigger
    {
        IUnityEventListenerModifier<TMCardModel> OnEffectEvent { get; }
    }
}