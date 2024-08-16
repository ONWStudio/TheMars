using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Onw.ServiceLocator;
using Onw.Attribute;
using Onw.Feedback;
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

        [field: SerializeField, ReadOnly] public bool IsAnimated { get; private set; }

        [field: Header("Card Controller")]
        // .. 패
        [field: SerializeField, SelectableSerializeField] public TMCardHandController CardHandController { get; private set; }
        // .. 덱
        [field: SerializeField, SelectableSerializeField] public TMCardDeckController CardDeckController { get; private set; }
        // .. 무덤
        [field: SerializeField, SelectableSerializeField] public TMCardTombController CardTombController { get; private set; }

        [field: Header("Manager Event")]
        [field: SerializeField] public SafeUnityEvent OnTurnEnd { get; private set; } = new();
        [field: SerializeField] public SafeUnityEvent<TMCardController> OnUsedCard { get; private set; } = new();

        [field: SerializeField] public TMCardCreator CardCreator { get; private set; } = new();

        [field: Header("Camera")]
        [field: SerializeField, SelectableSerializeField] public Camera CardSystemCamera { get; private set; }
        
        public IIgnorePlayFeedbackPlayer FeedbackPlayer => _feedbackPlayer;

        [Header("Card Importer")]
        [SerializeField]  private TMCardHandImporter _cardHandImporter = new();

        [SerializeField, InitializeRequireComponent] private FeedbackPlayer _feedbackPlayer;
        
        private void Awake()
        {
            OnTurnEnd.AddListener(ServiceMonoBehaviourHelper.GetService<TMDelayEffectManager>().OnNextTurn);

            if (!ServiceLocator<ITMCardService>.RegisterService(this))
            {
                ServiceLocator<ITMCardService>.ChangeService(this);
            }
            
            _feedbackPlayer.OnAddedFeedback.AddListener(feedbackCount =>
            {
                Debug.Log(feedbackCount);

                if (!_feedbackPlayer.IsPlaying)
                {
                    _feedbackPlayer.PlayEvents();
                }
            });
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
            if (FeedbackPlayer.IsPlaying) return; // .. 카드가 드로우 중이거나 카드를 사용중일때는 턴 종료 불가

            OnTurnEnd.Invoke();

            // .. 핸드에서 카드 건네받기
            List<TMCardController> cards = CardHandController.DequeueCards();

            cards.ForEach(card => card.OnTurnEnd());

            List<TMCardController> importerCards = _cardHandImporter.GetCards(DRAW_CARD_MAX); // .. 다음 턴에 무조건 나와야 할 카드부터 드로우
            
            importerCards.AddRange(CardDeckController.DequeueCards(DRAW_CARD_MAX - importerCards.Count)); // .. 무조건 나와야 할 카드 갯수만큼 제외해서 덱에서 드로우
            
            if (importerCards.Count < DRAW_CARD_MAX) // .. 덱에 카드가 부족하다면?
            {
                CardDeckController.PushCards(CardTombController.DequeueDeadCards());                          // .. 무덤에서 덱으로 카드 옮기기
                importerCards.AddRange(CardDeckController.DequeueCards(DRAW_CARD_MAX - importerCards.Count)); // .. 부족한 카드 수만 큼 다시 덱에서 뽑아오기 
            }
            
            foreach (var controller in importerCards)
            {
                List<MMF_Feedback> feedbacks = new()
                {
                    FeedbackCreator.CreateUnityEvent(() =>
                    {
                        CardHandController.AddCard(controller);
                        
                        controller.transform.localPosition = CardHandController.DeckTransform.localPosition;
                        controller.OnDrawBegin();
                    }),
                    
                };


            }


            
            CardHandController.(
                importerCards,
                controller => controller.OnDrawEnded());
        }

        public void AddListenerToCard(TMCardController controller)
        {
            controller.OnChangedParent.AddListener(parent =>
                controller.OnField = parent == CardHandController.transform);
        }

        public void OnClickCard(TMCardController controller)
        {
            OnUsedCard.Invoke(controller);
        }
    }
}