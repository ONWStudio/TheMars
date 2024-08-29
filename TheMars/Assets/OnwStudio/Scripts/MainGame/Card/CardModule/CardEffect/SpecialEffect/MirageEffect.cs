// using TMCard.Runtime;
// namespace TMCard.Effect
// {
//     /// <summary>
//     /// .. 신기루
//     /// </summary>
//     public sealed class MirageEffect : TMCardSpecialEffect
//     {
//         public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
//         {
//             controller.OnClickEvent.RemoveAllToAddListener(eventState =>
//             {
//                 trigger.OnEffectEvent.Invoke(eventState);
//                 controller.DisposeCard();
//             });
//
//             controller.OnTurnEndedEvent.RemoveAllToAddListener(eventState => controller.DestroyCard());
//         }
//
//         public MirageEffect() : base("Mirage") { }
//     }
// }
