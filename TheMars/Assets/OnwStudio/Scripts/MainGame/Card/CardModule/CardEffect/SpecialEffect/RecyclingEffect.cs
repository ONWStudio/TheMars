using TMCard.Runtime;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 재활용
    /// </summary>
    public sealed class RecyclingEffect : TMCardSpecialEffect
    {
        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                controller.RecycleToHand();
            });
        }

        public RecyclingEffect() : base("Recycling") {}
    }
}
