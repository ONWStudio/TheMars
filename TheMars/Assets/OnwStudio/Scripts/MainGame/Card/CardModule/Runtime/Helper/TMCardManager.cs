using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Event;
using Onw.Manager;
using Onw.Attribute;
using Onw.Extensions;
using Onw.UI.Components;
using Image = UnityEngine.UI.Image;

namespace TM.Card.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardManager : SceneSingleton<TMCardManager>
    {
        [System.Serializable]
        public struct TMCardManagerUI
        {
            [field: SerializeField, SelectableSerializeField] public HorizontalEnumeratedItem CardCollectIconScrollView { get; private set; }
            [field: SerializeField, SelectableSerializeField] public Image DeckImage { get; private set; }
            [field: SerializeField, SelectableSerializeField] public RectTransform CollectField { get; private set; }

            public void SetDragView(bool isOn)
            {
                CardCollectIconScrollView.SetActiveGameObject(isOn);
                DeckImage.SetActiveGameObject(!isOn);
                CollectField.SetActiveGameObject(!isOn);
            }
        }

        public event UnityAction<TMCardModel> OnAddedCardEvent
        {
            add => _onAddedCardEvent.AddListener(value);
            remove => _onAddedCardEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnRemovedCardEvent
        {
            add => _onRemovedCardEvent.AddListener(value);
            remove => _onRemovedCardEvent.RemoveListener(value);
        }
        
        [field: SerializeField] private ReactiveField<int> _maxCardCount = new() { Value = 10 };
        
        public IReadOnlyList<TMCardModel> Cards => _cards;
        public int CardCount => _cards.Count;

        [SerializeField, ReadOnly] private List<TMCardModel> _cards = new();
        [SerializeField] private UnityEvent<TMCardModel> _onAddedCardEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onRemovedCardEvent = new();
        
        protected override string SceneName => "MainGameScene";

        [field: FormerlySerializedAs("_uiComponents")]
        [field: SerializeField] public TMCardManagerUI UIComponents { get; private set; }

        [field: Header("Card Creator")]
        [field: SerializeField] public TMCardCreator CardCreator { get; private set; } = new();

        [field: SerializeField, SelectableSerializeField] public RectTransform HandTransform { get; private set; }
        [field: SerializeField, SelectableSerializeField] public RectTransform DeckTransform { get; private set; }

        [field: Header("Camera")]
        [field: SerializeField, SelectableSerializeField] public Camera CardSystemCamera { get; private set; }

        [field: SerializeReference, SerializeReferenceDropdown] public ITMCardSorter CardSorter { get; private set; }
        public IReactiveField<int> MaxCardCount => _maxCardCount;

        protected override void Init() {}
        
        private void Start()
        {
            UIComponents.SetDragView(true);

            CardCreator.OnPostCreateCard += addListenerForCard;

            if (CardCreator.TryCreateCardByKey("CreditSupplyStation", out TMCardModel card))
            {
                AddCard(card);
            }
        }

        private void addListenerForCard(TMCardModel card)
        {
            card.OnDragBeginCard += onDragBeginCard;
            card.OnDragEndCard += onDragEndCard;
        }

        public void AddCard(TMCardModel card)
        {
            if (_cards.Count >= _maxCardCount.Value || !card) return;
            
            _cards.Add(card);
            card.transform.SetParent(HandTransform, false);
            card.OnSellCard += RemoveCard;

            SortCards();
            _onAddedCardEvent.Invoke(card);
        }

        public void RemoveCard(TMCardModel card)
        {
            if (!_cards.Remove(card)) return;

            SortCards();
            _onRemovedCardEvent.Invoke(card);
        }

        public void SortCards()
        {
            CardSorter
                .SortCards(_cards, HandTransform)
                .ForEach(transformInfo => transformInfo.Target.CardBodyMover.TargetPosition = transformInfo.Position);
        }

        private void onDragBeginCard(TMCardModel card)
        {
            UIComponents.SetDragView(false);
            _cards
                .Where(someCard => someCard != card)
                .ForEach(someCard => someCard.gameObject.SetActive(false));
        }

        private void onDragEndCard(TMCardModel card)
        {
            UIComponents.SetDragView(true);
            _cards
                .Where(someCard => someCard != card)
                .ForEach(someCard => someCard.gameObject.SetActive(true));
        }
    }
}