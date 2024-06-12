using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using CoroutineExtensions;

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
        private const int DRAW_CARD_MAX = 5;

        public override string SceneName => "MainGameScene";

        public int HandCardCount => _cardHandUIController.CardCount;
        public int DeckCardCount => _cardDeckUIController.CardCount;

        [Header("Controller Object")]
        [SerializeField] private GameObject _deckUIObject = null;
        [SerializeField] private GameObject _tombUIObject = null;

        // .. 임포터
        [Header("Card Importer")]
        [SerializeField] private TMCardHandImporter _cardHandImporter = null;

        // .. 패
        [Header("Hand Controller")]
        [SerializeField] private TMCardHandUIController _cardHandUIController = null;
        // .. 덱
        private TMCardDeckUIController _cardDeckUIController = null;
        // .. 무덤
        private TMCardTombUIController _cardTombUIController = null;

        private bool _isDraw = false;

        protected override void Init()
        {
            _cardDeckUIController = _deckUIObject.AddComponent<TMCardDeckUIController>();
            _cardTombUIController = _tombUIObject.AddComponent<TMCardTombUIController>();
        }

        private void Start()
        {
            _cardHandImporter
                .PushCards(_cardDeckUIController.GetCards(DRAW_CARD_MAX));

            // .. 핸드에서 사용 한 카드를 받아 무덤에 넘겨줍니다
            _cardHandUIController
                .OnUseCardEnded
                .AddListener(decideUsedCardState);

            DrawCardFromDeck();
        }

        /// <summary>
        /// .. 카드가 파괴될 카드인지 무덤으로 갈 카드인지 판별합니다
        /// </summary>
        private void decideUsedCardState(TMCardUIController cardUI)
        {
            cardUI.transform.SetParent(_tombUIObject.transform, false);
        }

        public void DrawCardFromDeck()
        {
            if (_isDraw || _cardHandUIController.UsingCardCount > 0) return; // .. 카드가 드로우 중이거나 카드를 사용중일때는 드로우 불가

            _isDraw = true;

            // .. 핸드에서 카드 건네받기
            List<TMCardUIController> cards = _cardHandUIController.DequeueCards();
            foreach (TMCardUIController cardUI in cards)
            {
                // .. 카드 효과 비활성화 (이벤트 비활성화, 상호작용 비활성화)
                cardUI.SetOn(false);

                // .. 무덤으로 이동하는 애니메이션 연출
                MMF_Parallel parallelEvent = new();

                parallelEvent.Feedbacks.Add(EventCreator
                    .CreateSmoothPositionEvent(cardUI.gameObject, _cardHandUIController.TombTransform.localPosition));

                parallelEvent.Feedbacks.Add(EventCreator
                    .CreateSmoothRotationEvent(cardUI.transform, Vector3.zero));

                // .. TODO : 파괴되거나 회수 하는 경우 분기 
                MMF_Events mmfEvents = new()
                {
                    PlayEvents = new()
                };

                // .. 연출 후 실제 트랜스폼 무덤 오브젝트를 부모로 설정
                mmfEvents
                    .PlayEvents
                    .AddListener(() => cardUI.transform.SetParent(_tombUIObject.transform, false));

                cardUI
                    .EventReceiver
                    .PlayEvent(parallelEvent, mmfEvents); // .. 이벤트 시작
            }

            this.WaitCompletedConditions(
                () => cards.Count <= 0 || cards.All(cardUI => !cardUI.EventReceiver.IsPlaying), // .. 해당 조건이 만족된다면 콜백 호출
                () =>
                {
                    int count = DRAW_CARD_MAX; // .. 최대 드로우 갯수만큼 카드 드로우
                    List<TMCardUIController> importerCards = _cardHandImporter.GetCards(count); // .. 다음 턴에 무조건 나와야 할 카드부터 드로우

                    importerCards.AddRange(_cardDeckUIController.GetCards(count - importerCards.Count)); // .. 무조건 나와야 할 카드 갯수만큼 제외해서 덱에서 드로우
                    _cardHandUIController.SetCards(importerCards); // .. 손 패에 카드 세팅

                    this.WaitCompletedConditions(
                        () => importerCards.All(cardUI => cardUI.OnCard), // .. 카드 정렬 이벤트가 끝날때까지 대기
                        () => _isDraw = false); // .. 턴 끝내기 버튼 활성화
                });
        }
    }
}