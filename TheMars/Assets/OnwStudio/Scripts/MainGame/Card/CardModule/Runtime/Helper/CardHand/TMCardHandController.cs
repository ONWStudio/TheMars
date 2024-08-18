using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using MoreMountains.Feedbacks;
using Onw.Feedback;
using Onw.Attribute;
using Onw.ServiceLocator;

namespace TMCard.Runtime
{
    /// <summary>
    /// .. 카드 패 UI 들을 관리하는 클래스
    /// 카드 패들의 움직임과 상호작용에 관한 관리를 하는 클래스 입니다
    /// 카드를 추가하거나 제거할때 카드를 생성/파괴하는 것이 아닌 리스트의 자료구조에서만 제거합니다 카드의 생명주기는 외부에서 관리합니다
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardHandController : MonoBehaviour
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

        public int CardCount => _cardsOnHand.Count;

        [FormerlySerializedAs("cardSorter")]
        [Header("Sorter")]
        [SerializeReference, SerializeReferenceDropdown]
        private ICardSorter _cardSorter;

        [FormerlySerializedAs("_handOnCards")]
        [FormerlySerializedAs("handOnCards")]
        [FormerlySerializedAs("cards")] [SerializeField, FormerlySerializedAs("_cards")]
        private List<TMCardController> _cardsOnHand = new(5);

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
            _cardsOnHand.AddRange(cards);
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 끝 자리에 배치됩니다 자동으로 정렬됩니다 
        /// </summary>
        /// <param name="card"> .. 패에 추가 할 카드 </param>
        public void AddCard(TMCardController card)
        {
            setCard(card);
            _cardsOnHand.Add(card);
        }

        /// <summary>
        /// .. 카드 하나를 패에 추가합니다 카드는 가장 앞 자리에 배치됩니다
        /// </summary>
        /// <param name="card"></param>
        public void AddCardToFirst(TMCardController card)
        {
            setCard(card);
            _cardsOnHand.Insert(0, card);
        }

        /// <summary>
        /// .. 패에 존재하는 카드를 제거합니다 존재하지 않으면 제거하지 않습니다 자동으로 정렬됩니다
        /// </summary>
        /// <param name="card"> .. 제거 할 카드 객체 </param>
        public void RemoveCard(TMCardController card)
        {
            _cardsOnHand.Remove(card);
        }
        
        public MMF_Parallel SetCardsAndGetSortFeedbacks(List<TMCardController> cards, float duration = 1.0f)
        {
            SetCards(cards);
            return GetSortCardsFeedbacks(duration);
        }
        
        public MMF_Parallel AddCardToGetSortFeedbacks(TMCardController card, float duration = 1.0f)
        {
            AddCard(card);
            return GetSortCardsFeedbacks(duration);
        }

        public MMF_Parallel AddCardFirstToGetSortFeedbacks(TMCardController card, float duration = 1.0f)
        {
            AddCardToFirst(card);
            return GetSortCardsFeedbacks(duration);
        }

        public MMF_Parallel RemoveCardToGetSortFeedbacks(TMCardController card, float duration = 1.0f)
        {
            RemoveCard(card);
            return GetSortCardsFeedbacks(duration);
        }

        /// <summary>
        /// ..카드들을 패에서 모두 꺼내오는 메서드
        /// </summary>
        /// <returns> .. 패에서 꺼내온 카드들 </returns>
        public List<TMCardController> DequeueCards()
        {
            List<TMCardController> cards = _cardsOnHand.ToList();

            cards.ForEach(cardUI => cardUI.SetOn(false));

            _cardsOnHand.Clear();
            return cards;
        }

        /// <summary>
        /// .. 카드를 정렬해야하는 경우 해당 메서드를 호출하면 리스트에 존재하는 카드들을 올바른 위치로 정렬 시킵니다 이벤트 기반입니다
        /// </summary>
        public MMF_Parallel GetSortCardsFeedbacks(float duration = 1.0f)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return null;

            MMF_Parallel parallel = new();
            parallel.Feedbacks.Capacity = _cardsOnHand.Count + 1;

            for (int i = 0; i < _cardsOnHand.Count; i++)
            {
                PositionRotationInfo transformInfo = _cardSorter.ArrangeCard(_cardsOnHand, i, HandTransform);
                TMCardController card = transformInfo.Target;

                card.transform.SetAsLastSibling();
                parallel.Feedbacks.Add(FeedbackCreator.CreateSmoothPositionAndRotationEvent(
                        card.gameObject,
                        transformInfo.Position,
                        transformInfo.Rotation,
                        duration));
            }

            return parallel;
        }

        private void setCard(TMCardController card)
        {
            card.transform.SetParent(transform, false);
        }
    }
}
