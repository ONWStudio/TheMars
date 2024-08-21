using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Feedback;
using Onw.ServiceLocator;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public sealed class TMCardDrawEffect : ITMNormalEffect, ITMInitializeEffect<TMCardDrawEffectCreator>
    {
        public string Description => $"덱에서 카드 {_drawCount}개 드로우";
        
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