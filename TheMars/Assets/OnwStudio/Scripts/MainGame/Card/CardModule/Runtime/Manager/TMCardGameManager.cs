using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Manager;
using Onw.Event;
using Onw.State;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardGameManager : SceneSingleton<TMCardGameManager>
    {
        private const int ALL_CARD_MAX = 50;
        private const int DRAW_CARD_MAX = 5;

        public override string SceneName => "MainGameScene";

        public int HandCardCount => CardHandUIController.CardCount;
        public int DeckCardCount => CardDeckUIController.CardCount;
        public int TombCardCount => CardTombUIController.CardCount;

        [field: SerializeField, ReadOnly] public int AnimatedCardCount { get; private set; } = 0;

        [field: Header("Card Controller")]
        // .. 패
        [field: SerializeField] public TMCardHandUIController CardHandUIController { get; private set; } = null;
        // .. 덱
        [field: SerializeField] public TMCardDeckUIController CardDeckUIController { get; private set; } = null;
        // .. 무덤
        [field: SerializeField] public TMCardTombUIController CardTombUIController { get; private set; } = null;

        [field: Header("Manager Event")]
        [field: SerializeField] public UnityEvent OnTurnEnd { get; private set; } = new();
        [field: SerializeField] public UnityEvent<TMCardController> OnUsedCard { get; private set; } = new();

        [Header("Card Importer")]
        [SerializeField] private TMCardHandImporter _cardHandImporter = new();
        [SerializeField] private TMCardCreator _cardCreator = new();

        [Header("Camera")]
        [SerializeField] private Camera _cardSystemCamera = null;

        protected override void Init()
        {
            OnTurnEnd.AddListener(DelayEffectManager.Instance.OnNextTurn);
        }

        private void Start()
        {
            List<TMCardController> controllers = _cardCreator.CreateCards(ALL_CARD_MAX);
            controllers.ForEach(addListenerToCard);
            CardDeckUIController.PushCards(controllers);
            _cardHandImporter.PushCards(CardDeckUIController.DequeueCards(DRAW_CARD_MAX));
            TurnEndToDrawCardsFromDeck();
        }

        public void TurnEndToDrawCardsFromDeck()
        {
            if (AnimatedCardCount > 0) return; // .. 카드가 드로우 중이거나 카드를 사용중일때는 턴 종료 불가

            OnTurnEnd.Invoke();

            // .. 핸드에서 카드 건네받기
            List<TMCardController> cards = CardHandUIController.DequeueCards();
            drawCard(cards, 0);

            static void drawCard(List<TMCardController> cards, int index)
            {
                while (index < cards.Count)
                {
                    TMCardController card = cards[index];
                    Instance.notifyTurnEndToCard(card);

                    if (card.EventSender.IsPlaying)
                    {
                        card.EventSender.OnCompleted.AddListener(onSuccessEvent);
                        break;
                    }
                    else
                    {
                        index++;
                        continue;
                    }

                    void onSuccessEvent()
                    {
                        card.EventSender.OnCompleted.RemoveListener(onSuccessEvent);
                        drawCard(cards, index + 1); 
                    }
                }

                if (index >= cards.Count)
                {
                    Instance.drawCardsFromDeck();
                }
            }
        }

        private void drawCardsFromDeck()
        {
            int count = DRAW_CARD_MAX; // .. 최대 드로우 갯수만큼 카드 드로우
            List<TMCardController> importerCards = _cardHandImporter.GetCards(count); // .. 다음 턴에 무조건 나와야 할 카드부터 드로우

            importerCards.AddRange(CardDeckUIController.DequeueCards(count - importerCards.Count)); // .. 무조건 나와야 할 카드 갯수만큼 제외해서 덱에서 드로우

            if (importerCards.Count < DRAW_CARD_MAX) // .. 덱에 카드가 부족하다면?
            {
                CardDeckUIController.PushCards(CardTombUIController.DequeueDeadCards()); // .. 무덤에서 덱으로 카드 옮기기
                importerCards.AddRange(CardDeckUIController.DequeueCards(DRAW_CARD_MAX - importerCards.Count)); // .. 부족한 카드 수만 큼 다시 덱에서 뽑아오기 
            }

            foreach (TMCardController controller in importerCards)
            {
                controller.transform.localPosition = CardHandUIController.DeckTransform.localPosition;
                controller.OnDrawBegin();
            }

            CardHandUIController.PushCardsInSortQueue(
                importerCards,
                controller => controller.OnDrawEnded());
        }

        private void addListenerToCard(TMCardController controller)
        {
            controller.EventSender.OnPlay.AddListener(() =>
            {
                if (controller.EventSender.IsPlaying) return;

                controller.SetOn(false);
                if (AnimatedCardCount++ <= 0)
                {
                    CardHandUIController.SetOn(false);
                }
            });
            controller.EventSender.OnCompleted.AddListener(() =>
            {
                if ((--AnimatedCardCount) > 0) return;

                CardHandUIController.SetOn(true);
            });

            controller.OnChangedParent.AddListener(parent =>
                controller.OnField = parent == CardHandUIController.transform);
        }

        public void OnClickCard(TMCardController controller)
        {
            controller.OnUsed();
            OnUsedCard.Invoke(controller);
        }

        public void MoveToScreenCenterAfterToTomb(TMCardController controller)
        {
            CardHandUIController.RemoveCardAndSort(controller);

            List<MMF_Feedback> events = new()
            {
                getMoveToScreenCenterEvent(controller)
            };

            events.AddRange(getMoveToTombEvent(controller, 1.0f));
            controller.EventSender.PlayEvents(events);
        }

        public void CollectCard(TMCardController triggerCard, int collectCount)
        {
            setPositionNewCardToHand(
                triggerCard,
                _cardCreator.CreateCards(collectCount),
                Vector3.zero);
        }

        public void DrawCardFromDeck(TMCardController triggerCard, int drawCount)
        {
            if (DeckCardCount <= 0)
            {
                CardDeckUIController.PushCards(CardTombUIController.DequeueDeadCards());
            }

            setPositionNewCardToHand(
                triggerCard,
                CardDeckUIController.DequeueCards(drawCount),
                CardHandUIController.DeckTransform.localPosition,
                sortedController => sortedController.OnDrawEnded());
        }

        public void DestroyCard(TMCardController controller)
        {
            controller.EventSender.PlayEvents(getDestroyEvent(controller));
        }

        public void MoveToTomb(TMCardController controller)
        {
            controller.EventSender.PlayEvents(getMoveToTombEvent(controller));
            CardHandUIController.RemoveCardAndSort(controller);
        }

        public void RecycleToHand(TMCardController controller)
        {
            CardHandUIController.RemoveCardAndSort(controller);
            List<MMF_Feedback> events = new()
            {
                getMoveToScreenCenterEvent(controller),
                EventCreator.CreateUnityEvent(() => CardHandUIController.AddCardToFirstAndSort(controller))
            };

            controller.EventSender.PlayEvents(events);
        }

        public void DrawUse(TMCardController controller)
        {
            Vector3 keepPosition = controller.transform.localPosition;
            Vector3 keepEulerAngle = controller.transform.localRotation.eulerAngles;
            List<MMF_Feedback> events = new()
            {
                getMoveToScreenCenterEvent(controller),
                EventCreator.CreateSmoothPositionAndRotationEvent(controller.gameObject, keepPosition, keepEulerAngle, 0.8f)
            };

            controller.EventSender.QueueEvents(events);
        }

        public void OnContinuingSeconds(TMCardController card, float continuingSeconds, System.Action onSuccess)
        {
            DelayEffectManager.Instance.WaitForSecondsEffect(
                continuingSeconds,
                () =>
                {
                    Debug.Log("지속 시간 종료");
                    onSuccess.Invoke();
                },
                remainingTime => { });

            MoveToTombAndHide(card);
        }

        public void OnContinuingTurns(TMCardController card, int turn)
        {
            //DelayEffectManager.Instance.WaitForTurnCountEffect(
            //    turn,
            //    () => { },
            //    turnCount => cardUI.CardData.UseCard());

            //MoveToTombAndHide(cardUI);
        }

        public void DelaySeconds(TMCardController card, float delayTime)
        {
            DelayEffectManager.Instance.WaitForSecondsEffect(
                delayTime,
                () => setActiveDelayCard(card),
                remainingTime => { });

            MoveToTombAndHide(card);
        }

        public void DelayTurn(TMCardController card, int turnCount)
        {
            DelayEffectManager.Instance.WaitForTurnCountEffect(
                turnCount,
                () => setActiveDelayCard(card),
                remainingTurn => { });

            MoveToTombAndHide(card);
        }

        public void MoveToTombAndHide(TMCardController card)
        {
            List<MMF_Feedback> events = new();
            MMF_Parallel parallel = new();
            parallel.Feedbacks.AddElements(
                EventCreator.CreateSmoothPositionAndRotationEvent(
                    card.gameObject,
                    CardHandUIController.TombTransform.localPosition,
                    Vector3.zero, 1.0f),
                EventCreator.CreateUnityEvent(() => CardHandUIController.RemoveCardAndSort(card)));

            card.EventSender.PlayEvents(new List<MMF_Feedback>()
            {
                parallel,
                EventCreator.CreateUnityEvent(() => card.gameObject.SetActive(false))
            });
        }

        public void DisposeCard(TMCardController card)
        {
            CardHandUIController.RemoveCardAndSort(card);

            List<MMF_Feedback> events = new()
            {
                getMoveToScreenCenterEvent(card),
                EventCreator.CreateUnityEvent(()
                    => card.EventSender.PlayEvents(getDestroyEvent(card)))
            };

            card.EventSender.PlayEvents(events);
        }

        /// <summary>
        /// .. 카드에게 턴이 종료됐다는 걸 알립니다 카드 내부에서 턴이 종료되었을때 특수한 상황이나 카드 효과에 따라 올바른 이벤트 메서드를 호출합니다
        /// </summary>
        /// <param name="controller"></param>
        private void notifyTurnEndToCard(TMCardController controller)
        {
            controller.OnTurnEnd();
        }

        private void setActiveDelayCard(TMCardController controller)
        {
            controller.gameObject.SetActive(true);
            List<MMF_Feedback> events = new(getMoveToTombEvent(controller));
            controller.EventSender.PlayEvents(events);
        }

        private void setPositionNewCardToHand(TMCardController triggerCard, List<TMCardController> newCards, Vector3 spawn, System.Action<TMCardController> endedSortCall = null)
        {
            foreach (TMCardController card in newCards)
            {
                // .. 새로운 카드의 이동 효과는 효과를 발동한 카드의 이벤트로 적용합니다
                MMF_Parallel parallelEvent = new();
                addListenerToCard(card);

                parallelEvent.Feedbacks.Add(EventCreator.CreateUnityEvent(() =>
                    card.transform.localPosition = spawn));

                parallelEvent.Feedbacks.Add(getMoveToScreenCenterEvent(card));
                triggerCard.EventSender.QueueEvent(parallelEvent);
                CardHandUIController.PushCardInSortQueue(card, endedSortCall);
            }
        }

        private Vector3 getScreenCenter()
        {
            Vector2 targetWorldPosition = _cardSystemCamera.GetScreenCenterWorldPoint();

            return CardHandUIController
                .transform
                .InverseTransformPoint(new(targetWorldPosition.x, targetWorldPosition.y, 0f));
        }

        private MMF_Feedback getMoveToScreenCenterEvent(TMCardController controller)
        {
            Vector3 targetPosition = getScreenCenter();

            return EventCreator.CreateSmoothPositionAndRotationEvent(
                controller.gameObject,
                new(targetPosition.x, targetPosition.y, 0f),
                Vector3.zero);
        }

        private MMF_Feedback getMoveToUp(TMCardController controller) => EventCreator.CreateSmoothPositionAndRotationEvent(
                controller.gameObject,
                controller.transform.localPosition + (Vector3)(controller.transform.up * controller.RectTransform.rect.size * 0.5f),
                Vector3.zero);

        private List<MMF_Feedback> getDestroyEvent(TMCardController controller) => new()
        {
            getMoveToUp(controller),
            EventCreator.CreateUnityEvent(() => Destroy(controller.gameObject))
        };

        private List<MMF_Feedback> getMoveToTombEvent(TMCardController controller, float duration = 0.5f) => new()
        {
            EventCreator.CreateSmoothPositionAndRotationEvent(
                controller.gameObject,
                CardHandUIController.TombTransform.localPosition,
                Vector3.zero, duration),
            EventCreator.CreateUnityEvent(
                () => CardTombUIController.EnqueueDeadCard(controller))
        };
    }
}