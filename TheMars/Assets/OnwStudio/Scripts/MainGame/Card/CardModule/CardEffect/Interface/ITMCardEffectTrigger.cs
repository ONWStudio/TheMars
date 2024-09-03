using Onw.Event;
namespace TMCard.Effect
{
    public interface ITMCardEffectTrigger
    {
        SafeUnityEvent OnEffectEvent { get; }
    }
}