// using TMCard.Runtime;
// namespace TMCard.Effect
// {
//     /// <summary>
//     /// .. 일회용
//     /// </summary>
//     public sealed class DisposableEffect : TMCardSpecialEffect
//     {
//         public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
//         {
//             controller.OnClickEvent.RemoveAllListener();
//             controller.OnClickEvent.AddListener(eventState =>
//             {
//                 trigger.OnEffectEvent.Invoke(eventState);
//                 controller.DisposeCard();
//             });
//         }
//
//         public DisposableEffect() : base("Disposable") { }
//     }
// }
