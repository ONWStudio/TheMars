using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.ServiceLocator;
using Onw.Attribute;
using Onw.Event;
using TMPro;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardManager : MonoBehaviour, ITMCardService
    {
        [System.Serializable]
        public struct TMCardManagerUI
        {
            [field: SerializeField, SelectableSerializeField] public Button DrawButton { get; private set; }
            [field: SerializeField, SelectableSerializeField] public TextMeshProUGUI DrawButtonText { get; private set; }
            [field: SerializeField, SelectableSerializeField] public Image DeckImage { get; private set; }
            [field: SerializeField, SelectableSerializeField] public Image HandImage { get; private set; }
            
            [field: Header("Hand Sprite Option")]
            [field: SerializeField] public Sprite HandNormalSprite { get; private set; }
            [field: SerializeField] public Sprite HandTombSprite { get; private set; }
            
            public void SetDragView(bool isOn)
            {
                DrawButton.image.enabled = isOn;
                DrawButton.enabled = isOn;
                DrawButtonText.enabled = isOn;
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

        [SerializeField] private TMCardManagerUI _uiComponents;
        
        private void Awake()
        {
            if (!ServiceLocator<ITMCardService>.RegisterService(this))
            {
                ServiceLocator<ITMCardService>.ChangeService(this);
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
            card.OnDragBeginCard.AddListener(() => _uiComponents.SetDragView(false));
            card.OnDragEndCard.AddListener(() => _uiComponents.SetDragView(true));
            
            foreach (PositionRotationInfo transformInfo in CardSorter.SortCards(_cards, HandTransform))
            {
                transformInfo.Target.CardBodyMover.TargetPosition = transformInfo.Position;
            }
        }

        private void OnDestroy()
        {
            ServiceLocator<ITMCardService>.ClearService();
        }
    }
}