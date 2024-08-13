using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Helper;
using Onw.Event;

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
        private readonly struct SortCardEventPair
        {
            public TMCardController Owner { get; }
            public Action<TMCardController> EndedCallback { get; }

            public SortCardEventPair(TMCardController owner, Action<TMCardController> endedCallback)
            {
                Owner = owner;
                EndedCallback = endedCallback;
            }
        }

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

        private readonly Queue<SortCardEventPair> _sortQueue = new();
        private bool _hasSortingCard = false;

        private void Awake()
        {
            _cardSorter ??= new CycleForm
            {
                MaxAngle = 128f,
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
        /// <param name="card"> .. 패에 추가 할 카드 </param>
        public void AddCard(TMCardController card)
        {
            setCard(card);
            _cards.Add(card);
        }

        public void AddCardAndSort(TMCardController card)
        {
            AddCard(card);
            SortCards();
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 앞 자리에 배치됩니다
        /// </summary>
        /// <param name="card"></param>
        public void AddCardToFirst(TMCardController card)
        {
            setCard(card);
            _cards.Insert(0, card);
        }

        public void AddCardToFirstAndSort(TMCardController card)
        {
            AddCardToFirst(card);
            SortCards();
        }

        /// <summary>
        /// .. 패에 존재하는 카드를 제거합니다 존재하지 않으면 제거하지 않습니다 자동으로 정렬됩니다
        /// </summary>
        /// <param name="card"> .. 제거 할 카드 객체 </param>
        public void RemoveCard(TMCardController card)
        {
            _cards.Remove(card);
        }

        public void RemoveCardAndSort(TMCardController card)
        {
            RemoveCard(card);
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
        public void SortCards(float duration = 1.0f, Action<TMCardController> onEachSuccess = null, Action onAllSuccess = null)
        {
            int count = 0;
            int targetCount = _cards.Count;

            for (int i = 0; i < _cards.Count; i++)
            {
                sortCard(_cardSorter.ArrangeCard(_cards, i, HandTransform), duration, card =>
                {
                    onEachSuccess?.Invoke(card);
                    count++;

                    if (targetCount == count)
                    {
                        onAllSuccess?.Invoke();
                    }
                });
            }
        }

        private void sortCard(PositionRotationInfo transformInfo, float duration, Action<TMCardController> endedSortCall)
        {
            TMCardController card = transformInfo.Target;

            card.transform.SetAsLastSibling();
            card.EventSender.QueueEvent(
                EventCreator.CreateSmoothPositionAndRotationEvent(
                    card.gameObject,
                    transformInfo.Position,
                    transformInfo.Rotation,
                    duration));
            card.EventSender.OnCompleted.AddListener(onSuccessSort);

            void onSuccessSort()
            {
                card.EventSender.OnCompleted.RemoveListener(onSuccessSort);
                endedSortCall?.Invoke(card);
            }
        }

        public void PushCardsInSortQueue(List<TMCardController> controllers, Action<TMCardController> endedSortCall = null)
        {
            controllers.ForEach(controller => PushCardInSortQueue(controller, endedSortCall: endedSortCall));
        }

        public void PushCardInSortQueue(TMCardController controller, Action<TMCardController> endedSortCall = null)
        {
            _sortQueue.Enqueue(new(controller, endedSortCall));
            setCard(controller);

            // .. 큐에 더 이상 정렬 대기중인 카드가 없을때
            if (!_hasSortingCard)
            {
                sortCard(this);
            }

            static void sortCard(TMCardHandUIController handController)
            {
                Debug.Log(handController._sortQueue.Count);

                handController._hasSortingCard = handController._sortQueue.TryDequeue(out SortCardEventPair selectedCard);

                if (handController._hasSortingCard)
                {
                    handController._cards.Add(selectedCard.Owner);
                    handController.SortCards(0.5f, selectedCard.EndedCallback, () =>
                    {
                        selectedCard.EndedCallback?.Invoke(selectedCard.Owner);
                        sortCard(handController);
                    });
                }
            }
        }

        private void setCard(TMCardController card)
        {
            card.transform.SetParent(transform, false);
        }
    }
}
