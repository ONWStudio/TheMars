using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Onw.Manager;
using Onw.Attribute;
using Onw.Event;
using Onw.Extensions;
using Onw.UI.Components;
using UniRx;
using Image = UnityEngine.UI.Image;
using TM.Runtime.UI;
using UnityEngine.Events;

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
            [field: SerializeField, SelectableSerializeField] public Image HandImage { get; private set; }
            [field: SerializeField, SelectableSerializeField] public RectTransform CollectField { get; private set; }

            public void SetDragView(bool isOn)
            {
                CardCollectIconScrollView.SetActiveGameObject(isOn);
                DeckImage.SetActiveGameObject(!isOn);
                CollectField.SetActiveGameObject(!isOn);
                HandImage.enabled = isOn;
            }
        }

        protected override string SceneName => "MainGameScene";

        public event UnityAction<TMCardModel> OnAddedCardEvent
        {
            add => _onAddedCardEvent.AddListener(value);
            remove => _onRemovedCardEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnRemovedCardEvent
        {
            add => _onRemovedCardEvent.AddListener(value);
            remove => _onRemovedCardEvent.RemoveListener(value);
        }


        [field: FormerlySerializedAs("_uiComponents")]
        [field: SerializeField] public TMCardManagerUI UIComponents { get; private set; }

        [field: Header("Card Creator")]
        [field: SerializeField] public TMCardCreator CardCreator { get; private set; } = new();

        [field: SerializeField, SelectableSerializeField] public RectTransform HandTransform { get; private set; }
        [field: SerializeField, SelectableSerializeField] public RectTransform DeckTransform { get; private set; }

        [field: Header("Camera")]
        [field: SerializeField, SelectableSerializeField] public Camera CardSystemCamera { get; private set; }

        [field: SerializeReference, SerializeReferenceDropdown] public ITMCardSorter CardSorter { get; private set; }

        [field: SerializeField] public int MaxCardCount { get; private set; } = 10;
        
        public IReadOnlyList<TMCardModel> Cards => _cards;
        public int CardCount => _cards.Count;
        

        [SerializeField, ReadOnly] private List<TMCardModel> _cards = new();
        [SerializeField] private UnityEvent<TMCardModel> _onAddedCardEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onRemovedCardEvent = new();

        protected override void Init() {}
        
        private void Start()
        {
            UIComponents.SetDragView(true);

            CardCreator.OnPostCreateCard += addListenerForCard;
            AddCard(CardCreator.CreateRandomCard());
        }

        private void addListenerForCard(TMCardModel card)
        {
            card.OnDragBeginCard += onDragBeginCard;
            card.OnDragEndCard += onDragEndCard;
        }

        public void AddCard(TMCardModel card)
        {
            if (_cards.Count >= MaxCardCount || !card) return;
            
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