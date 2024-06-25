using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CoroutineExtensions;
using OnwAttributeExtensions;
using MoreMountains.Feedbacks;
using TcgEngine;

namespace TMCardUISystemModules
{
    /// <summary>
    /// .. 카드 시스템은 모듈화가 되어 있습니다. 더 마르스의 시뮬레이션 기능과 관계없이 카드를 사용하고 이벤트를 호출하고 연출을 일으키는 기능만 담당하고 있습니다
    /// 카드 시스템은 하나의 모듈처럼 동작하게 설계되어있어 특정 컴포넌트만 따로 사용시 에러가 발생할 수 있습니다
    /// 카드 시스템은 덱 or HandImporter(다음 턴에 무조건 나와야 하는 카드) -> 패 -> 무덤 순으로 이동하며 사용 시 파괴되거나 무덤으로 가거나 다시 덱으로 이동할 수 있습니다
    /// </summary>
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

        [Header("Card Importer")]
        [SerializeField] private TMCardHandImporter _cardHandImporter = null;

        [Header("Camera")]
        [SerializeField] private Camera _cardSystemCamera = null;

        protected override void Init()
        {
            OnTurnEnd.AddListener(DelayEffectManager.Instance.OnNextTurn);
        }

        private void Start()
        {
            List<TMCardUIController> cards = TMCardUICreator.Instance.CreateCards(ALL_CARD_MAX);
            cards.ForEach(addListenerToCard);
            CardDeckUIController.PushCards(cards);
            _cardHandImporter.PushCards(CardDeckUIController.DequeueCards(DRAW_CARD_MAX));
            DrawCardFromDeck();
        }

        public void DrawCardFromDeck()
        {
            if (AnimatedCardCount > 0) return; // .. 카드가 드로우 중이거나 카드를 사용중일때는 드로우 불가

            OnTurnEnd.Invoke();

            // .. 핸드에서 카드 건네받기
            List<TMCardUIController> cards = CardHandUIController.DequeueCards();
            cards.ForEach(notifyTurnEndToCard); // .. 카드에게 턴이 종료됐다고 알려주기

            this.WaitCompletedConditions(
                () => AnimatedCardCount <= 0, // .. 해당 조건이 만족된다면 콜백 호출
                () =>
                {
                    int count = DRAW_CARD_MAX; // .. 최대 드로우 갯수만큼 카드 드로우
                    List<TMCardUIController> importerCards = _cardHandImporter.GetCards(count); // .. 다음 턴에 무조건 나와야 할 카드부터 드로우

                    importerCards.AddRange(CardDeckUIController.DequeueCards(count - importerCards.Count)); // .. 무조건 나와야 할 카드 갯수만큼 제외해서 덱에서 드로우

                    if (importerCards.Count < DRAW_CARD_MAX) // .. 덱에 카드가 부족하다면?
                    {
                        CardDeckUIController.PushCards(CardTombUIController.DequeueDeadCards()); // .. 무덤에서 덱으로 카드 옮기기
                        importerCards.AddRange(CardDeckUIController.DequeueCards(DRAW_CARD_MAX - importerCards.Count)); // .. 부족한 카드 수만 큼 다시 덱에서 뽑아오기 
                    }

                    CardHandUIController.SetCards(importerCards); // .. 손 패에 카드 세팅
                });
        }

        private void addListenerToCard(TMCardUIController card)
        {
            card.EventSender.OnStartBeginEvent.AddListener(() =>
            {
                if (card.EventSender.IsPlaying) return;
                AnimatedCardCount++;
            });
            card.EventSender.OnComplitedEndEvent.AddListener(() => AnimatedCardCount--);

            card.OnClickCard.AddListener(card => card.OnUseStart());
            card.OnMoveToScreenCenter.AddListener(onMoveToScreenCenter);
            card.OnMoveToTomb.AddListener(moveToTomb);
            card.OnRecycleToHand.AddListener(onRecycleToHand);
            card.OnDrawUse.AddListener(onDrawUse);
            card.OnDelaySeconds.AddListener(onDelaySeconds);
            card.OnDelayTurn.AddListener(onDelayTurn);
            card.OnHoldCard.AddListener(onHoldCard);
            card.OnDestroyCard.AddListener(onDestroyCard);
        }

        private void onMoveToScreenCenter(TMCardUIController cardUI)
        {
            CardHandUIController.RemoveCard(cardUI);

            cardUI.SetOn(false);

            Vector3 targetWorldPosition = _cardSystemCamera
                .ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.5f));

            Vector3 targetPosition = cardUI
                .transform
                .parent
                .InverseTransformPoint(new(targetWorldPosition.x, targetWorldPosition.y, 0f));

            List<MMF_Feedback> events = new()
            {
                EventCreator.CreateSmoothPositionAndRotationEvent(
                    cardUI.gameObject,
                    new Vector3(targetPosition.x, targetPosition.y, 0f),
                    Vector3.zero),
                EventCreator.CreateUnityEvent(cardUI.OnUseEnded, null, null, null)
            };

            cardUI.EventSender.PlayEvents(events);
        }

        /// <summary>
        /// .. 카드에게 턴이 종료됐다는 걸 알립니다 카드 내부에서 턴이 종료되었을때 특수한 상황이나 카드 효과에 따라 올바른 이벤트 메서드를 호출합니다
        /// </summary>
        /// <param name="cardUI"></param>
        private void notifyTurnEndToCard(TMCardUIController cardUI)
        { 
            cardUI.OnTurnEnd();
        }

        /// <summary>
        /// .. 카드를 무덤으로 보냅니다
        /// </summary>
        private void moveToTomb(TMCardUIController cardUI)
        {
            List<MMF_Feedback> events = new()
            {
                EventCreator.CreateSmoothPositionAndRotationEvent(
                    cardUI.gameObject,
                    CardHandUIController.TombTransform.localPosition,
                    Vector3.zero),
                EventCreator.CreateUnityEvent(
                    () => CardTombUIController.EnqueueDeadCard(cardUI),
                    null,
                    null,
                    null)
            };

            cardUI.EventSender.PlayEvents(events);
        }

        private void onRecycleToHand(TMCardUIController cardUI)
        {

            CardHandUIController.AddCardToFirst(cardUI);
        }

        private void onDrawUse(TMCardUIController cardUI)
        {
            CardHandUIController.RemoveCard(cardUI);
            onMoveToScreenCenter(cardUI);
        }

        private void onDelaySeconds(TMCardUIController cardUI, float delayTime)
        {
            DelayEffectManager.Instance.WaitForSecondsEffect(
                delayTime,
                () => setActiveDelayCard(cardUI),
                remainingTime => { });

            cardUI.gameObject.SetActive(false);
        }

        private void onDelayTurn(TMCardUIController cardUI, int turnCount)
        {
            DelayEffectManager.Instance.WaitForTurnCountEffect(
                turnCount,
                () => setActiveDelayCard(cardUI),
                remainingTurn => { });

            cardUI.gameObject.SetActive(false);
        }

        private void onHoldCard(TMCardUIController cardUI, string friendlyCardID)
        {
            TMCardUIController friendlyCard = CardHandUIController.GetCardFromID(friendlyCardID);
            // TODO : 보유 효과 처리
        }

        private void onDestroyCard(TMCardUIController cardUI)
        {
            Vector3 targetPosition = cardUI.transform.localPosition + (Vector3)(cardUI.transform.up * cardUI.RectTransform.rect.size * 0.5f);
            List<MMF_Feedback> events = new()
            {
                EventCreator.CreateSmoothPositionAndRotationEvent(cardUI.gameObject, targetPosition, Vector3.zero),
                EventCreator.CreateUnityEvent(() => Destroy(cardUI.gameObject), null, null, null)
            };

            cardUI.EventSender.PlayEvents(events);
        }

        private void setActiveDelayCard(TMCardUIController cardUI)
        {
            cardUI.gameObject.SetActive(true);
            moveToTomb(cardUI);
        }
    }
}