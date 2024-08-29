// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using MoreMountains.Feedbacks;
// using Onw.Feedback;
// using Onw.Extensions;
// using Onw.ServiceLocator;
// using Object = UnityEngine.Object;
// using System.Threading.Tasks;
//
// namespace TMCard.Runtime
// {
//     public static class TMCardControllerExtensions
//     {
//         public static void MoveToScreenCenterAfterToTomb(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//
//             MMF_Parallel parallel = new();
//             parallel.Feedbacks.Add(service.CardHandController.RemoveCardToGetSortFeedbacks(card));
//             parallel.Feedbacks.Add(card.GetMoveToScreenCenterEvent());
//
//             List<MMF_Feedback> feedbacks = new()
//             {
//                 parallel,
//                 card.GetMoveToTombEvent(1.0f),
//                 FeedbackCreator.CreateUnityEvent(
//                     () => service.CardTombController.EnqueueDeadCard(card))
//             };
//
//             service.FeedbackPlayer.EnqueueEvents(feedbacks);
//         }
//
//         public static void DrawCardFromDeck(this TMCardController triggerCard, int drawCount)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//
//             if (service.DeckCardCount <= 0)
//             {
//                 service.CardDeckController.PushCards(service.CardTombController.DequeueDeadCards());
//             }
//
//             triggerCard.SetPositionNewCardToHand(
//                 service.CardDeckController.DequeueCards(drawCount),
//                 service.CardHandController.DeckTransform.localPosition,
//                 sortedController => sortedController.OnDrawEnded());
//         }
//
//         public static void DestroyCard(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             service.FeedbackPlayer.EnqueueEvents(card.GetDestroyEvent());
//         }
//
//         public static void MoveToTomb(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             MMF_Parallel parallel = service.CardHandController.RemoveCardToGetSortFeedbacks(card);
//             parallel.Feedbacks.Add(card.GetMoveToTombEvent());
//
//             service.FeedbackPlayer.EnqueueEvents(new() 
//             { 
//                 parallel, 
//                 FeedbackCreator.CreateUnityEvent(
//                     () => service.CardTombController.EnqueueDeadCard(card))
//             });
//         }
//         
//         public static void DrawUse(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             Vector3 keepPosition = card.transform.localPosition;
//             Vector3 keepEulerAngle = card.transform.localRotation.eulerAngles;
//
//             List<MMF_Feedback> events = new List<MMF_Feedback>
//             {
//                 card.GetMoveToScreenCenterEvent(),
//                 FeedbackCreator.CreateSmoothPositionAndRotationEvent(card.gameObject, keepPosition, keepEulerAngle, 0.8f)
//             };
//
//             service.FeedbackPlayer.EnqueueEvents(events);
//         }
//
//
//         public static void MoveToTombAndHide(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             MMF_Parallel parallel = service.CardHandController.RemoveCardToGetSortFeedbacks(card);
//             parallel.Feedbacks.Add(FeedbackCreator.CreateSmoothPositionAndRotationEvent(
//                 card.gameObject,
//                 service.CardHandController.TombTransform.localPosition,
//                 Vector3.zero));
//
//             service.FeedbackPlayer.EnqueueEvents(new List<MMF_Feedback>
//             {
//                 parallel,
//                 FeedbackCreator.CreateUnityEvent(() => card.gameObject.SetActive(false))
//             });
//         }
//
//         public static void DisposeCard(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             MMF_Parallel parallel = service.CardHandController.RemoveCardToGetSortFeedbacks(card);
//             parallel.Feedbacks.Add(card.GetMoveToScreenCenterEvent());
//             
//             service.FeedbackPlayer.EnqueueEvents(new() 
//             { 
//                 parallel,
//                 FeedbackCreator.CreateUnityEvent(() => 
//                 {
//                     service.FeedbackPlayer.EnqueueEventToHead(
//                         card.GetMoveToUp(),
//                         FeedbackCreator.CreateUnityEvent(() => Object.Destroy(card.gameObject)));
//                 })
//             });
//         }
//         
//         public static void SetActiveDelayCard(this TMCardController card)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             MMF_Parallel parallel = new();
//             parallel.Feedbacks.Add(FeedbackCreator.CreateUnityEvent(() => card.gameObject.SetActive(true)));
//             parallel.Feedbacks.Add(card.GetMoveToTombEvent());
//
//             service.FeedbackPlayer.EnqueueEvents(new() 
//             {
//                 parallel,
//                 FeedbackCreator.CreateUnityEvent(() => service.CardTombController.EnqueueDeadCard(card))
//             });
//         }
//
//         public static void SetPositionNewCardToHand(this TMCardController triggerCard, List<TMCardController> newCards, Vector3 spawn, Action<TMCardController> endedSortCall = null)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
//             
//             foreach (TMCardController card in newCards)
//             {
//                 // .. 새로운 카드의 이동 효과는 효과를 발동한 카드의 이벤트로 적용합니다
//                 MMF_Parallel parallelEvent = new();
//
//                 parallelEvent.Feedbacks.Add(FeedbackCreator.CreateUnityEvent(() =>
//                     card.transform.localPosition = spawn));
//
//                 parallelEvent.Feedbacks.Add(card.GetMoveToScreenCenterEvent());
//
//                 service.FeedbackPlayer.EnqueueEventsToHead(new() 
//                 {
//                     parallelEvent,
//                     service.CardHandController.AddCardToGetSortFeedbacks(card),
//                     FeedbackCreator.CreateUnityEvent(() => endedSortCall?.Invoke(card))
//                 });
//             }
//         }
//
//         public static MMF_Parallel GetMoveToScreenCenterEvent(this TMCardController card, float duration = 1.0f)
//         {
//             Vector3 targetRotation = card.transform.localRotation.eulerAngles;
//             float keepDuration = duration;
//             duration = 0f;
//             
//             if (!TMCardControllerHelper.TryGetScreenCenter(out Vector3 targetPosition))
//             {
//                 targetPosition = card.transform.localPosition;
//             }
//             else
//             {
//                 targetRotation = Vector3.zero;
//                 duration = keepDuration;
//             }
//
//             return FeedbackCreator.CreateSmoothPositionAndRotationEvent(
//                 card.gameObject,
//                 new(targetPosition.x, targetPosition.y, 0f),
//                 targetRotation,
//                 duration);
//         }
//
//         public static MMF_Parallel GetMoveToUp(this TMCardController card) => FeedbackCreator.CreateSmoothPositionAndRotationEvent(
//                 card.gameObject,
//                 card.transform.localPosition + (Vector3)(card.transform.up * card.RectTransform.rect.size * 0.5f),
//                 Vector3.zero);
//
//         public static List<MMF_Feedback> GetDestroyEvent(this TMCardController card) => new()
//         {
//             card.GetMoveToUp(),
//             FeedbackCreator.CreateUnityEvent(() => Object.Destroy(card.gameObject))
//         };
//
//         public static MMF_Parallel GetMoveToTombEvent(this TMCardController card, float duration = 0.5f)
//         {
//             return ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service) ? FeedbackCreator.CreateSmoothPositionAndRotationEvent(
//                 card.gameObject,
//                 service.CardHandController.TombTransform.localPosition,
//                 Vector3.zero, duration) : null;
//         }
//     }
//
//     public static class TMCardControllerHelper
//     {
//         public static bool TryGetScreenCenter(out Vector3 centerPosition)
//         {
//             if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service))
//             {
//                 centerPosition = Vector3.zero;
//                 return false;
//             }
//             
//             Vector2 targetWorldPosition = service.CardSystemCamera.GetScreenCenterWorldPoint();
//
//             centerPosition = service
//                 .CardHandController
//                 .transform
//                 .InverseTransformPoint(new(targetWorldPosition.x, targetWorldPosition.y, 0f));
//             
//             return true;
//         }
//     }
// }
