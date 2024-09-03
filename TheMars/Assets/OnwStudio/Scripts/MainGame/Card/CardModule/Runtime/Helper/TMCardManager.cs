using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.ServiceLocator;
using Onw.Attribute;
using Onw.Event;
using UnityEngine.Serialization;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardManager : MonoBehaviour
    {
        [System.Serializable]
        public struct TMCardManagerUI
        {
            [field: SerializeField, SelectableSerializeField] public Image DeckImage { get; private set; }
            [field: SerializeField, SelectableSerializeField] public Image HandImage { get; private set; }
            
            [field: Header("Hand Sprite Option")]
            [field: SerializeField] public Sprite HandNormalSprite { get; private set; }
            [field: SerializeField] public Sprite HandTombSprite { get; private set; }
            
            public void SetDragView(bool isOn)
            {
                DeckImage.sprite = isOn ? HandNormalSprite : HandTombSprite;
                HandImage.enabled = isOn;
            }
        }
        
        [field: Header("Card Creator")]
        [field: SerializeField] public TMCardCreator CardCreator { get; private set; } = new();

        [field: SerializeField, SelectableSerializeField] public RectTransform HandTransform { get; private set; }
        [field: SerializeField, SelectableSerializeField] public RectTransform DeckTransform { get; private set; }
        
        [field: Header("Camera")]
        [field: SerializeField, SelectableSerializeField] public Camera CardSystemCamera { get; private set; }
        
        [field: SerializeReference, SerializeReferenceDropdown] public ITMCardSorter CardSorter { get; private set; }
        [field: SerializeField] public SafeUnityEvent<TMCardModel> OnUsedCard { get; private set; } = new();

        public IReadOnlyList<TMCardModel> Cards => _cards;
        
        [SerializeField, ReadOnly] private List<TMCardModel> _cards = new();

        [field: FormerlySerializedAs("_uiComponents")]
        [field: SerializeField] public TMCardManagerUI UIComponents { get; private set; }

        private void Awake()
        {
            if (!ServiceLocator<TMCardManager>.RegisterService(this))
            {
                ServiceLocator<TMCardManager>.ChangeService(this);
            }
        }

        private void Start()
        {
            AddCard(CardCreator.CreateCard());
        }

        public void AddCard(TMCardModel card)
        {
            _cards.Add(card);
            card.transform.SetParent(HandTransform, false);
            card.OnDragBeginCard.AddListener(onDragBeginCard);
            card.OnDragEndCard.AddListener(onDragEndCard);
            
            CardSorter
                .SortCards(_cards, HandTransform)
                .ForEach(transformInfo => transformInfo.Target.CardBodyMover.TargetPosition = transformInfo.Position);
        }

        public void RemoveCard(TMCardModel card)
        {
            _cards.Remove(card);
            card.OnDragBeginCard.RemoveListener(onDragBeginCard);
            card.OnDragEndCard.RemoveListener(onDragEndCard);
            
            CardSorter
                .SortCards(_cards, HandTransform)
                .ForEach(transformInfo => transformInfo.Target.CardBodyMover.TargetPosition = transformInfo.Position);
        }

        private void onDragBeginCard()
        {
            UIComponents.SetDragView(false);
        }

        private void onDragEndCard()
        {
            UIComponents.SetDragView(true);
        }

        private void OnDestroy()
        {
            ServiceLocator<TMCardManager>.ClearService();
        }
    }
}