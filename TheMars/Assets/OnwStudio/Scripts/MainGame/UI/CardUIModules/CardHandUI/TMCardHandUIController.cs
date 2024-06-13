using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CoroutineExtensions;
using SubClassSelectorSpace;
using TcgEngine;

namespace TMCardUISystemModules
{
    /// <summary>
    /// .. 카드 패 UI 들을 관리하는 클래스
    /// 카드 패들의 움직임과 상호작용에 관한 관리를 하는 클래스 입니다
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardHandUIController : MonoBehaviour
    {
        [field: Header("Transform")]
        [field: SerializeField] public RectTransform DeckTransform { get; private set; }
        [field: SerializeField] public RectTransform HandTransform { get; private set; }
        [field: SerializeField] public RectTransform TombTransform { get; private set; }

        public int CardCount => _cards.Count;

        [SerializeReference, SubClassSelector(typeof(ICardSorter))] private ICardSorter _cardSorter = null;

        [Header("Cards")]
        [SerializeField] private List<TMCardUIController> _cards = new(10);

        private void Awake()
        {
            _cardSorter ??= new SectorForm
            {
                MaxAngle = 128f,
                HeightRatioFromWidth = 0.25f
            };
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
            cards.ForEach(setCardUI);
            _cards.AddRange(cards);
            SortCards();
        }

        public void AddCard(TMCardUIController cardUI)
        {
            setCardUI(cardUI);

            _cards.Add(cardUI);
            SortCards();
        }

        public void AddCardToFirst(TMCardUIController cardUI)
        {
            setCardUI(cardUI);

            _cards.Insert(0, cardUI);
            SortCards();
        }

        public void RemoveCard(TMCardUIController cardUI)
        {
            _cards.Remove(cardUI);
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

        private void setCardUI(TMCardUIController cardUI)
        {
            cardUI.transform.SetParent(transform, false);
            cardUI.transform.localPosition = DeckTransform.localPosition;
        }

        /// <summary>
        /// .. 카드 패의 형태를 결정하는 UI의 세팅을 합니다
        /// </summary>
        /// <param name="cardSoter"> .. CardSorter는 카드를 어떤 형태로 정렬시킬지 정하는 인터페이스 입니다 셋터는 한번 호출후 다시 결정할 수 없습니다 .. 인스펙터에서 변경 가능 </param>
        public void SetHandUI(ICardSorter cardSoter)
        {
            _cardSorter ??= cardSoter;
        }

        public void SortCards()
        {
            _cards.ForEach(cardUI => cardUI.SetOn(false));
            _cardSorter.SortCards(_cards, HandTransform);

            // .. 카드의 정렬이 끝날때까지 상호작용 불가
            this.WaitCompletedConditions(
                () => _cards.All(card => !card.EventReceiver.IsPlaying),
                () => _cards.ForEach(card => card.SetOn(true)));
        }
    }
}
