using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.ServiceLocator;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Event;
using Onw.Extensions;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardManager : MonoBehaviour
    {
        [System.Serializable]
        public struct TMCardManagerUI
        {
            [field: SerializeField, SelectableSerializeField] public ScrollRect CardCollectIconScrollView { get; private set; }
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

        public IReadOnlyList<TMCardModel> Cards => _cards;

        [SerializeField, ReadOnly] private List<TMCardModel> _cards = new();

        private void Awake()
        {
            if (ServiceLocator<TMCardManager>.RegisterService(this)) return;

            ServiceLocator<TMCardManager>.ChangeService(this);
        }

        private void Start()
        {
            CardCreator.OnCreateCard.AddListener(addListenerForCard);
            AddCard(CardCreator.CreateCard());
        }

        private void addListenerForCard(TMCardModel card)
        {
            card.OnDragBeginCard.AddListener(onDragBeginCard);
            card.OnDragEndCard.AddListener(onDragEndCard);
        }

        public void AddCard(TMCardModel card)
        {
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

        private void OnDestroy()
        {
            ServiceLocator<TMCardManager>.ClearService();
        }
    }
}