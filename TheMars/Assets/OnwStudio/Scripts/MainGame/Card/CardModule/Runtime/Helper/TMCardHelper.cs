using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.ServiceLocator;
using Onw.Attribute;
using Onw.Event;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardHelper : MonoBehaviour, ITMCardService
    {
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