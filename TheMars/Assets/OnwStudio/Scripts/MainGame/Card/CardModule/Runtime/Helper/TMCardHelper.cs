using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using MoreMountains.Feedbacks;
using Onw.ServiceLocator;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Event;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardHelper : MonoBehaviour, ITMCardService
    {
        private const int ALL_CARD_MAX = 50;
        private const int DRAW_CARD_MAX = 5;

        public int HandCardCount => CardHandController.CardCount;
        public int DeckCardCount => CardDeckController.CardCount;
        public int TombCardCount => CardTombController.CardCount;

        [field: SerializeField, ReadOnly] public int AnimatedCardCount { get; private set; } = 0;

        [field: Header("Card Controller")]
        // .. 패
        [field: SerializeField, SelectableSerializeField] public TMCardHandController CardHandController { get; private set; } = null;
        // .. 덱
        [field: SerializeField, SelectableSerializeField] public TMCardDeckController CardDeckController { get; private set; } = null;
        // .. 무덤
        [field: SerializeField, SelectableSerializeField] public TMCardTombController CardTombController { get; private set; } = null;

        [field: Header("Manager Event")]
        [field: SerializeField] public UnityEvent OnTurnEnd { get; private set; } = new();
        [field: SerializeField] public UnityEvent<TMCardController> OnUsedCard { get; private set; } = new();

        [field: SerializeField] public TMCardCreator CardCreator { get; private set; } = new();
        
        [field: Header("Camera")]
        [field: SerializeField, SelectableSerializeField] public Camera CardSystemCamera { get; private set; } = null;

        [Header("Card Importer")]
        [SerializeField]  private TMCardHandImporter _cardHandImporter = new();

        private void Awake()
        {
            OnTurnEnd.AddListener(ServiceMonoBehaviourHelper.GetService<TMDelayEffectManager>().OnNextTurn);

            if (!ServiceLocator<ITMCardService>.RegisterService(this))
            {
                ServiceLocator<ITMCardService>.ChangeService(this);
            }
        }

        private void Start()
        {
            List<TMCardController> controllers = CardCreator.CreateCards(ALL_CARD_MAX);
            CardDeckController.PushCards(controllers);
            _cardHandImporter.PushCards(CardDeckController.DequeueCards(DRAW_CARD_MAX));
            TurnEndToDrawCardsFromDeck();
        }

        private void OnDestroy()
        {
            ServiceLocator<ITMCardService>.ClearService();
        }

        public void TurnEndToDrawCardsFromDeck()
        {
            if (AnimatedCardCount > 0) return; // .. 카드가 드로우 중이거나 카드를 사용중일때는 턴 종료 불가

            OnTurnEnd.Invoke();

            // .. 핸드에서 카드 건네받기
            List<TMCardController> cards = CardHandController.DequeueCards();
            drawCard(this, cards, 0);
            return;

            static void drawCard(TMCardHelper manager, List<TMCardController> cards, int index)
            {
                while (index < cards.Count)
                {
                    var card = cards[index];
                    card.OnTurnEnd();

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
                        drawCard(manager, cards, index + 1); 
                    }
                }

                if (index >= cards.Count)
                {
                    List<TMCardController> importerCards = _cardHandImporter.GetCards(DRAW_CARD_MAX); // .. 다음 턴에 무조건 나와야 할 카드부터 드로우

                    importerCards.AddRange(CardDeckController.DequeueCards(DRAW_CARD_MAX - importerCards.Count)); // .. 무조건 나와야 할 카드 갯수만큼 제외해서 덱에서 드로우

                    if (importerCards.Count < DRAW_CARD_MAX) // .. 덱에 카드가 부족하다면?
                    {
                        CardDeckController.PushCards(CardTombController.DequeueDeadCards());                          // .. 무덤에서 덱으로 카드 옮기기
                        importerCards.AddRange(CardDeckController.DequeueCards(DRAW_CARD_MAX - importerCards.Count)); // .. 부족한 카드 수만 큼 다시 덱에서 뽑아오기 
                    }

                    foreach (var controller in importerCards)
                    {
                        controller.transform.localPosition = CardHandController.DeckTransform.localPosition;
                        controller.OnDrawBegin();
                    }

                    CardHandController.PushCardsInSortQueue(
                        importerCards,
                        controller => controller.OnDrawEnded());
                }
            }
        }

        public void AddListenerToCard(TMCardController controller)
        {
            controller.EventSender.OnPlay.AddListener(() =>
            {
                if (controller.EventSender.IsPlaying) return;

                controller.SetOn(false);
                if (AnimatedCardCount++ <= 0)
                {
                    CardHandController.SetOn(false);
                }
            });
            controller.EventSender.OnCompleted.AddListener(() =>
            {
                if ((--AnimatedCardCount) > 0) return;

                CardHandController.SetOn(true);
            });

            controller.OnChangedParent.AddListener(parent =>
                controller.OnField = parent == CardHandController.transform);
        }

        public void OnClickCard(TMCardController controller)
        {
            controller.OnUsed();
            OnUsedCard.Invoke(controller);
        }
    }
}