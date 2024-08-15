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
                TMCardHelper.Instance.RecycleToHand(controller);
            });
        }

        public RecyclingEffect() : base("Recycling") {}
    }
}
