using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Onw.Feedback;
using Onw.Extensions;
using Onw.ServiceLocator;
using Object = UnityEngine.Object;

namespace TMCard.Runtime
{
    public static class TMCardControllerExtensions
    {
        public static void MoveToScreenCenterAfterToTomb(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            service.CardHandController.RemoveCardAndSort(card);

            List<MMF_Feedback> events = new()
            {
                card.GetMoveToScreenCenterEvent()
            };

            events.AddRange(card.GetMoveToTombEvent(1.0f));
            service.FeedbackPlayer.QueueEvents(events);
        }

        public static void CollectCard(this TMCardController triggerCard, int collectCount)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            triggerCard.SetPositionNewCardToHand(
                service.CardCreator.CreateCards(collectCount),
                Vector3.zero);
        }

        public static void DrawCardFromDeck(this TMCardController triggerCard, int drawCount)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;

            if (service.DeckCardCount <= 0)
            {
                service.CardDeckController.PushCards(service.CardTombController.DequeueDeadCards());
            }

            triggerCard.SetPositionNewCardToHand(
                service.CardDeckController.DequeueCards(drawCount),
                service.CardHandController.DeckTransform.localPosition,
                sortedController => sortedController.OnDrawEnded());
        }

        public static void DestroyCard(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            service.FeedbackPlayer.QueueEvents(card.GetDestroyEvent());
        }

        public static void MoveToTomb(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            service.FeedbackPlayer.QueueEvents(card.GetMoveToTombEvent());
            service.CardHandController.RemoveCardAndSort(card);
        }

        public static void RecycleToHand(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            service.CardHandController.RemoveCardAndSort(card);
            List<MMF_Feedback> events = new List<MMF_Feedback>
            {
                card.GetMoveToScreenCenterEvent(),
                FeedbackCreator.CreateUnityEvent(() => service.CardHandController.AddCardToFirstAndSort(card))
            };

            card.EventSender.PlayEvents(events);
        }

        public static void DrawUse(this TMCardController card)
        {
            var keepPosition = card.transform.localPosition;
            var keepEulerAngle = card.transform.localRotation.eulerAngles;
            List<MMF_Feedback> events = new List<MMF_Feedback>
            {
                card.GetMoveToScreenCenterEvent(),
                FeedbackCreator.CreateSmoothPositionAndRotationEvent(card.gameObject, keepPosition, keepEulerAngle, 0.8f)
            };

            card.EventSender.QueueEvents(events);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void OnContinuingSeconds(this TMCardController card, float continuingSeconds, Action onSuccess)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service) || 
                !ServiceLocator<TMDelayEffectManager>.TryGetService(out var delayEffectManager)) return;
            
            delayEffectManager.WaitForSecondsEffect(
                continuingSeconds,
                () =>
                {
                    Debug.Log("지속 시간 종료");
                    onSuccess.Invoke();
                },
                remainingTime => { });

            card.MoveToTombAndHide();
        }

        public static void OnContinuingTurns(TMCardController card, int turn)
        {
            //DelayEffectManager.Instance.WaitForTurnCountEffect(
            //    turn,
            //    () => { },
            //    turnCount => cardUI.CardData.UseCard());

            //MoveToTombAndHide(cardUI);
        }

        public static void DelaySeconds(TMCardController card, float delayTime)
        {
            if (!ServiceLocator<TMDelayEffectManager>.TryGetService(out var service)) return;
            
            service.WaitForSecondsEffect(
                delayTime,
                card.SetActiveDelayCard,
                remainingTime => { });

            card.MoveToTombAndHide();
        }



        public static void MoveToTombAndHide(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            var parallel = new MMF_Parallel();
            parallel.Feedbacks.AddElements(
                FeedbackCreator.CreateSmoothPositionAndRotationEvent(
                    card.gameObject,
                    service.CardHandController.TombTransform.localPosition,
                    Vector3.zero),
                FeedbackCreator.CreateUnityEvent(() => service.CardHandController.RemoveCardAndSort(card)));

            service.FeedbackPlayer.QueueEvents(new List<MMF_Feedback>
            {
                parallel,
                FeedbackCreator.CreateUnityEvent(() => card.gameObject.SetActive(false))
            });
        }

        public static void DisposeCard(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            service.CardHandController.RemoveCardAndSort(card);

            List<MMF_Feedback> events = new()
            {
                card.GetMoveToScreenCenterEvent(),
            };
            
            events.AddRange(card.GetDestroyEvent());
            service.FeedbackPlayer.QueueEvents(events);
        }
        
        public static void SetActiveDelayCard(this TMCardController card)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            card.gameObject.SetActive(true);
            List<MMF_Feedback> events = new(card.GetMoveToTombEvent());
            service.FeedbackPlayer.QueueEvents(events);
        }

        public static void SetPositionNewCardToHand(this TMCardController triggerCard, List<TMCardController> newCards, Vector3 spawn, Action<TMCardController> endedSortCall = null)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            
            foreach (var card in newCards)
            {
                // .. 새로운 카드의 이동 효과는 효과를 발동한 카드의 이벤트로 적용합니다
                MMF_Parallel parallelEvent = new();

                parallelEvent.Feedbacks.Add(FeedbackCreator.CreateUnityEvent(() =>
                    card.transform.localPosition = spawn));

                parallelEvent.Feedbacks.Add(card.GetMoveToScreenCenterEvent());
                service.FeedbackPlayer.QueueEventToHead(parallelEvent);
                service.CardHandController.PushCardInSortQueue(card, endedSortCall);
            }
        }

        public static MMF_Feedback GetMoveToScreenCenterEvent(this TMCardController card)
        {
            var targetRotation = card.transform.localRotation.eulerAngles;
            float duration = 0f;
            
            if (!TMCardControllerHelper.TryGetScreenCenter(out var targetPosition))
            {
                targetPosition = card.transform.localPosition;
            }
            else
            {
                targetRotation = Vector3.zero;
                duration = 1.0f;
            }

            return FeedbackCreator.CreateSmoothPositionAndRotationEvent(
                card.gameObject,
                new(targetPosition.x, targetPosition.y, 0f),
                targetRotation,
                duration);
        }

        public static MMF_Feedback GetMoveToUp(this TMCardController card) => FeedbackCreator.CreateSmoothPositionAndRotationEvent(
                card.gameObject,
                card.transform.localPosition + (Vector3)(card.transform.up * card.RectTransform.rect.size * 0.5f),
                Vector3.zero);

        public static List<MMF_Feedback> GetDestroyEvent(this TMCardController card) => new()
        {
            card.GetMoveToUp(),
            FeedbackCreator.CreateUnityEvent(() => Object.Destroy(card.gameObject))
        };

        public static List<MMF_Feedback> GetMoveToTombEvent(this TMCardController card, float duration = 0.5f)
        {
            List<MMF_Feedback> feedbacks = new();

            if (ServiceLocator<ITMCardService>.TryGetService(out var service))
            {
                feedbacks.AddElements(FeedbackCreator.CreateSmoothPositionAndRotationEvent(
                        card.gameObject,
                        service.CardHandController.TombTransform.localPosition,
                        Vector3.zero, duration),
                    FeedbackCreator.CreateUnityEvent(
                        () => service.CardTombController.EnqueueDeadCard(card)));
                
            }

            return feedbacks;
        }
    }

    public static class TMCardControllerHelper
    {
        public static bool TryGetScreenCenter(out Vector3 centerPosition)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out var service))
            {
                centerPosition = Vector3.zero;
                return false;
            }
            
            var targetWorldPosition = service.CardSystemCamera.GetScreenCenterWorldPoint();

            centerPosition = service.CardHandController
                .transform
                .InverseTransformPoint(new(targetWorldPosition.x, targetWorldPosition.y, 0f));
            
            return true;
        }
    }
}
