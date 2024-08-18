using TMCard.Runtime;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 신기루
    /// </summary>
    public sealed class MirageEffect : TMCardSpecialEffect
    {
        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() =>
            {
                trigger.OnEffectEvent.Invoke();
                controller.DisposeCard();
            });

            controller.OnTurnEndedEvent.RemoveAllToAddListener(() => controller.DestroyCard());
        }

        public MirageEffect() : base("Mirage") { }
    }
}
