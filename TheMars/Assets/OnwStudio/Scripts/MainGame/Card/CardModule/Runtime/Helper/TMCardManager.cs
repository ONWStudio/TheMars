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

namespace TM.Card.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardManager : SceneSingleton<TMCardManager>
    {
        protected override string SceneName => "MainGameScene";
        
        [System.Serializable]
        public struct TMCardManagerUI
        {
            [field: SerializeField, SelectableSerializeField] public HorizontalEnumeratedItem CardCollectIconScrollView { get; private set; }
            [field: SerializeField, SelectableSerializeField] public Image DeckImage { get; private set; }
            [field: SerializeField, SelectableSerializeField] public Image HandImage { get; private set; }

            [field: Header("Hand Sprite Option")]
            [field: SerializeField] public Sprite HandNormalSprite { get; private set; }
            [field: SerializeField] public Sprite HandTombSprite { get; private set; }

            public void SetDragView(bool isOn)
            {
                CardCollectIconScrollView.gameObject.SetActive(isOn);
                DeckImage.sprite = isOn ? HandNormalSprite : HandTombSprite;
                HandImage.enabled = isOn;
            }
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

        protected override void Init() {}
        
        private void Start()
        {
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

            SortCards();
        }

        public void RemoveCard(TMCardModel card)
        {
            _cards.Remove(card);

            SortCards();
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