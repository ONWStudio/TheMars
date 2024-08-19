using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Feedback;
using Onw.ServiceLocator;
using TMCard.Runtime;

namespace TMCard.Effect
{
    // .. TODO : 드로우 효과로 드로우 카드 획득 시 두장 겹쳐서 나오는 버그, 중간중간 알 수 없는 이유로 화면 중앙으로 이동하지 않고 바로 패로 이동하는 
    public sealed class TMCardDrawEffect : ITMNormalEffect, ITMInitializeEffect<TMCardDrawEffectCreator>
    {
        [SerializeField, ReadOnly]
        private int _drawCount;

        public void Initialize(TMCardDrawEffectCreator effectCreator)
        {
            _drawCount = effectCreator.DrawCount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            
            trigger.OnEffectEvent.AddListener(eventState =>
            {
                if (eventState == CardEventState.TURN_END)
                {
                    if (service.DeckCardCount <= 0)
                    {
                        service.CardDeckController.PushCards(service.CardTombController.DequeueDeadCards());
                    }
                    
                    List<TMCardController> cards = service.CardDeckController.DequeueCards(_drawCount);

                    foreach (TMCardController card in cards)
                    {
                        MMF_Parallel parallelEvent = new();

                        parallelEvent.Feedbacks.Add(FeedbackCreator.CreateUnityEvent(() =>
                        {
                            card.transform.SetParent(service.CardHandController.transform, false); 
                            card.transform.localPosition = Vector3.zero;
                        }));

                        parallelEvent.Feedbacks.Add(card.GetMoveToScreenCenterEvent());

                        service.FeedbackPlayer.EnqueueEventsToHead(new() 
                        {
                            parallelEvent,
                            card.GetMoveToTombEvent(),
                            FeedbackCreator.CreateUnityEvent(() =>
                            {
                                card.OnDrawEnded();
                                service.CardTombController.EnqueueDeadCard(card);
                            })
                        });
                    }
                }
                else
                {
                    controller.DrawCardFromDeck(_drawCount);
                }
            });
        }
    }
}