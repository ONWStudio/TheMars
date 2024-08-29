// using Onw.Feedback;
// using Onw.ServiceLocator;
// using TMCard.Runtime;
// namespace TMCard.Effect
// {
//     /// <summary>
//     /// .. 재활용
//     /// </summary>
//     public sealed class RecyclingEffect : TMCardSpecialEffect
//     {
//         public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
//         {
//             controller.OnClickEvent.RemoveAllToAddListener(eventState =>
//             {
//                 trigger.OnEffectEvent.Invoke(eventState);
//                 recycleToHand(controller);
//             });
//             
//             static void recycleToHand(TMCardController card)
//             {
//                 if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//                 MMF_Parallel parallel = service.CardHandController.RemoveCardToGetSortFeedbacks(card);
//                 parallel.Feedbacks.Add(card.GetMoveToScreenCenterEvent());
//
//                 service.FeedbackPlayer.EnqueueEvents(new() 
//                 {
//                     parallel,
//                     service.CardHandController.AddCardFirstToGetSortFeedbacks(card)
//                 });
//             }
//         }
//
//         public RecyclingEffect() : base("Recycling") {}
//     }
// }
