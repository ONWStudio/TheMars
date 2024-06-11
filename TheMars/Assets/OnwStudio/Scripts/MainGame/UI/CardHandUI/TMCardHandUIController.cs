using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CoroutineExtensions;
using SubClassSelectorSpace;

namespace TMCardUISystemModules
{
    /// <summary>
    /// .. 카드 패 UI 들을 관리하는 클래스
    /// 카드 패들의 움직임과 상호작용에 관한 관리를 하는 클래스 입니다
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardHandUIController : MonoBehaviour
    {
        [field: Header("Event")]
        [field: SerializeField] public UnityEvent<TMCardUIController> OnUseCardStarted { get; private set; } = new();
        [field: SerializeField] public UnityEvent<TMCardUIController> OnUseCardEnded { get; private set; } = new();

        [field: Header("Transform")]
        [field: SerializeField] public RectTransform DeckTransform { get; private set; }
        [field: SerializeField] public RectTransform HandTransform { get; private set; }
        [field: SerializeField] public RectTransform TombTransform { get; private set; }

        public int CardCount => _cards.Count;
        public int UsingCardCount { get; private set; } = 0;

        [SerializeReference, SubClassSelector(typeof(ICardSorter))] private ICardSorter _cardSorter = null;

        [Header("Cards")]
        [SerializeField] private List<TMCardUIController> _cards = new(10);

        private RectTransform _rectTransform = null;

        private void Awake()
        {
            _cardSorter ??= new SectorForm
            {
                MaxAngle = 128f,
                HeightRatioFromWidth = 0.25f
            };

            _rectTransform = transform as RectTransform;
        }

        private void Start()
        {
            UniRxObserver.ObserveInfomation(
                this,
                selector => _cardSorter,
                sorter => sorter.SortCards(_cards, HandTransform));
        }

        public void SetCards(List<TMCardUIController> cards)
        {
            foreach (TMCardUIController card in cards)
            {
                card.SetCardUI(transform);
                card.transform.localPosition = DeckTransform.localPosition;
                card.OnUseStarted.AddListener(onUseCardStarted);
                card.OnUseEnded.AddListener(onUseCardEnded);
            }

            _cards.AddRange(cards);
            sortCards(_cards);
        }

        public void SetOn(bool isOn)
        {
            _cards.ForEach(card => card.SetOn(isOn));
        }

        public List<TMCardUIController> DequeueCards()
        {
            List<TMCardUIController> cards = _cards.ToList();
            _cards.Clear();

            return cards;
        }

        /// <summary>
        /// .. 카드 패의 형태를 결정하는 UI의 세팅을 합니다
        /// </summary>
        /// <param name="cardSoter"> .. CardSorter는 카드를 어떤 형태로 정렬시킬지 정하는 인터페이스 입니다 셋터는 한번 호출후 다시 결정할 수 없습니다 .. 인스펙터에서 변경 가능 </param>
        public void SetHandUI(ICardSorter cardSoter)
        {
            _cardSorter ??= cardSoter;
        }

        private void sortCards(List<TMCardUIController> cards)
        {
            _cards.ForEach(cardUI => cardUI.SetOn(false));
            _cardSorter.SortCards(cards, HandTransform);

            // .. 카드의 정렬이 끝날때까지 상호작용 불가
            this.WaitCompletedConditions(
                () => cards.All(card => !card.EventReceiver.IsPlaying),
                () => cards.ForEach(card => card.SetOn(true)));
        }

        private void onUseCardStarted(TMCardUIController cardUI)
        {
            UsingCardCount++;

            _cards.Remove(cardUI);
            OnUseCardStarted.Invoke(cardUI);

            sortCards(_cards);
        }

        private void onUseCardEnded(TMCardUIController cardUI)
        {
            UsingCardCount--;
            OnUseCardEnded.Invoke(cardUI);
        }
    }
}
