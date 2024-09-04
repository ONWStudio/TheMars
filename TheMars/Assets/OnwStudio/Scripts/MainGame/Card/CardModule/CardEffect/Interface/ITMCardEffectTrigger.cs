using Onw.Event;
namespace TMCard.Effect
{
    public interface ITMCardEffectTrigger
    {
        IUnityEventListenerModifier OnEffectEvent { get; }
    }
}