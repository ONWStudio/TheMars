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

        public ICardSorter CardSorter
        {
            get => _cardSorter;
            set => _cardSorter ??= value;
        }

        public int CardCount => _cards.Count;

        [Header("Sorter")]
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

        /// <summary>
        /// .. 카드를 여러개 배치 시킵니다 자동으로 카드를 올바르게 정렬합니다
        /// </summary>
        /// <param name="cards"> .. 패에 세팅할 카드들 </param>
        public void SetCards(List<TMCardUIController> cards)
        {
            cards.ForEach(setCardUI);
            _cards.AddRange(cards);
            SortCards();
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 끝 자리에 배치됩니다 자동으로 정렬됩니다 
        /// </summary>
        /// <param name="cardUI"> .. 패에 추가 할 카드 </param>
        public void AddCard(TMCardUIController cardUI)
        {
            setCardUI(cardUI);

            _cards.Add(cardUI);
            SortCards();
        }

        public TMCardUIController GetCardFromID(string id)
        {
            return _cards.SingleOrDefault(cardUI => cardUI.CardData.Guid == id);
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 앞 자리에 배치됩니다 자동으로 정렬됩니다
        /// </summary>
        /// <param name="cardUI"></param>
        public void AddCardToFirst(TMCardUIController cardUI)
        {
            setCardUI(cardUI);

            _cards.Insert(0, cardUI);
            SortCards();
        }

        /// <summary>
        /// .. 패에 존재하는 카드를 제거합니다 존재하지 않으면 제거하지 않습니다 자동으로 정렬됩니다
        /// </summary>
        /// <param name="cardUI"> .. 제거 할 카드 객체 </param>
        public void RemoveCard(TMCardUIController cardUI)
        {
            _cards.Remove(cardUI);
            SortCards();
        }

        /// <summary>
        /// .. 패에 존재하는 모든 카드의 상호작용 상태를 세팅하는 메서드입니다
        /// </summary>
        /// <param name="isOn"> .. 카드의 상호작용 상태를 결정하는 boolen 값 </param>
        public void SetOn(bool isOn)
        {
            _cards.ForEach(card => card.SetOn(isOn));
        }

        /// <summary>
        /// ..카드들을 패에서 모두 꺼내오는 메서드
        /// </summary>
        /// <returns> .. 패에서 꺼내온 카드들 </returns>
        public List<TMCardUIController> DequeueCards()
        {
            List<TMCardUIController> cards = _cards.ToList();
            _cards.Clear();

            return cards;
        }

        /// <summary>
        /// .. 카드를 정렬해야하는 경우 해당 메서드를 호출하면 리스트에 존재하는 카드들을 올바른 위치로 정렬 시킵니다 이벤트 기반입니다
        /// </summary>
        public void SortCards()
        {
            _cards.ForEach(cardUI => cardUI.SetOn(false));
            _cardSorter.SortCards(_cards, HandTransform);

            // .. 카드의 정렬이 끝날때까지 상호작용 불가
            this.WaitCompletedConditions(
                () => _cards.All(card => !card.EventSender.IsPlaying),
                () =>
                {
                    foreach (TMCardUIController cardUI in _cards)
                    {
                        cardUI.SetOn(true);
                        cardUI.OnDrawEnded();
                    }
                });
        }

        private void setCardUI(TMCardUIController cardUI)
        {
            cardUI.transform.SetParent(transform, false);
            cardUI.transform.localPosition = DeckTransform.localPosition;
            cardUI.OnDrawBegin();
        }
    }
}
