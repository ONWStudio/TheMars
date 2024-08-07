using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Coroutine;
using Onw.Attribute;
using Onw.Extensions;

namespace TMCard.Runtime
{
    /// <summary>
    /// .. 카드 패 UI 들을 관리하는 클래스
    /// 카드 패들의 움직임과 상호작용에 관한 관리를 하는 클래스 입니다
    /// 카드를 추가하거나 제거할때 카드를 생성/파괴하는 것이 아닌 리스트의 자료구조에서만 제거합니다 카드의 생명주기는 외부에서 관리합니다
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardHandUIController : MonoBehaviour
    {
        [field: Header("Transform")]
        [field: SerializeField, SelectableSerializeField] public RectTransform DeckTransform { get; private set; }
        [field: SerializeField, SelectableSerializeField] public RectTransform HandTransform { get; private set; }
        [field: SerializeField, SelectableSerializeField] public RectTransform TombTransform { get; private set; }

        public ICardSorter CardSorter
        {
            get => _cardSorter;
            set => _cardSorter ??= value;
        }

        public int CardCount => _cards.Count;

        [Header("Sorter")]
        [SerializeReference, SerializeReferenceDropdown] private ICardSorter _cardSorter = null;

        [SerializeField] private List<TMCardController> _cards = new(5);

        private void Awake()
        {
            _cardSorter ??= new SectorForm
            {
                MaxAngle = 128f,
                HeightRatioFromWidth = 0.25f
            };
        }

        /// <summary>
        /// .. 카드를 여러개 배치 시킵니다
        /// </summary>
        /// <param name="cards"> .. 패에 세팅할 카드들 </param>
        public void SetCards(List<TMCardController> cards)
        {
            cards.ForEach(setCard);
            _cards.AddRange(cards);
        }

        public void SetCardsAndSort(List<TMCardController> cards)
        {
            SetCards(cards);
            SortCards();
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 끝 자리에 배치됩니다 자동으로 정렬됩니다 
        /// </summary>
        /// <param name="cardUI"> .. 패에 추가 할 카드 </param>
        public void AddCard(TMCardController cardUI)
        {
            setCard(cardUI);
            _cards.Add(cardUI);
        }

        public void AddCardAndSort(TMCardController cardUI)
        {
            AddCard(cardUI);
            SortCards();
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 앞 자리에 배치됩니다
        /// </summary>
        /// <param name="cardUI"></param>
        public void AddCardToFirst(TMCardController cardUI)
        {
            setCard(cardUI);
            _cards.Insert(0, cardUI);
        }

        public void AddCardToFirstAndSort(TMCardController cardUI)
        {
            AddCardToFirst(cardUI);
            SortCards();
        }

        /// <summary>
        /// .. 패에 존재하는 카드를 제거합니다 존재하지 않으면 제거하지 않습니다 자동으로 정렬됩니다
        /// </summary>
        /// <param name="cardUI"> .. 제거 할 카드 객체 </param>
        public void RemoveCard(TMCardController cardUI)
        {
            _cards.Remove(cardUI);
        }

        public void RemoveCardAndSort(TMCardController cardUI)
        {
            RemoveCard(cardUI);
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
        public List<TMCardController> DequeueCards()
        {
            List<TMCardController> cards = _cards.ToList();

            cards.ForEach(cardUI => cardUI.SetOn(false));

            _cards.Clear();
            return cards;
        }

        /// <summary>
        /// .. 카드를 정렬해야하는 경우 해당 메서드를 호출하면 리스트에 존재하는 카드들을 올바른 위치로 정렬 시킵니다 이벤트 기반입니다
        /// </summary>
        public void SortCards(float duration = 1.0f, System.Action<TMCardController> endedSortCall = null)
        {
            _cardSorter.SortCards(_cards, HandTransform, duration);
            _cards.ForEach(card => card.transform.SetAsLastSibling());

            // .. 카드의 정렬이 끝날때까지 상호작용 불가
            this.WaitCompletedConditions(
                () => _cards.All(card => !card.EventSender.IsPlaying),
                () => _cards.ToArray().ForEach(cardUI => endedSortCall?.Invoke(cardUI)));
        }

        public void SortCardsInOrder(int startIndex = 0, System.Action<TMCardController> endedSortCall = null)
        {
            StartCoroutine(iESortCardInOrder(startIndex, _cards, endedSortCall));

            IEnumerator iESortCardInOrder(int startIndex, IEnumerable<TMCardController> cards, System.Action<TMCardController> endedSortCall)
            {
                List<TMCardController> cardList = cards.ToList();

                for (int i = startIndex; i < cardList.Count; i++)
                {
                    TMCardController pivot = cardList[i];
                    WaitUntil waitUntil = new(() => !pivot.EventSender.IsPlaying);

                    _cardSorter.ArrangeCard(_cards, i, HandTransform, 0.5f);
                    yield return waitUntil;
                    endedSortCall?.Invoke(pivot);
                    yield return waitUntil; // .. 카드 UI가 드로우 중이라면
                }
            }
        }

        public void PushCardsAndSortInOrder(IEnumerable<TMCardController> controllers, System.Action<TMCardController> endedSortCall = null)
        {
            StartCoroutine(iEPushCardsAndSortInOrder(controllers, endedSortCall));

            IEnumerator iEPushCardsAndSortInOrder(IEnumerable<TMCardController> controllers, System.Action<TMCardController> endedSortCall)
            {
                foreach (TMCardController controller in controllers)
                {
                    WaitUntil waitUntil = new(() => !controller.EventSender.IsPlaying);

                    _cards.Add(controller);
                    setCard(controller);
                    int index = _cards.Count - 1;
                    _cardSorter.SortCards(_cards, HandTransform, 0.5f);
                    yield return waitUntil;
                    endedSortCall?.Invoke(controller);
                    yield return waitUntil;
                }
            }
        }

        private void setCard(TMCardController cardUI)
        {
            cardUI.transform.SetParent(transform, false);
        }
    }
}
