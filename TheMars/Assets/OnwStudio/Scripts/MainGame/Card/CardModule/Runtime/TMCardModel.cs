using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Onw.Event;
using Onw.Attribute;
using Onw.Components.Movement;
using Onw.ServiceLocator;
using Onw.UI.Components;
using TM;
using TMCard.Effect;

namespace TMCard.Runtime
{
    // .. Model
    [DisallowMultipleComponent]
    public sealed class TMCardModel : MonoBehaviour, ITMCardEffectTrigger
    {
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; }

        /// <summary>
        /// .. 카드가 현재 손 패 위에 있는지 확인하는 값입니다
        /// </summary>
        [field: Header("Runtime Option")]
        [field: SerializeField, ReadOnly] public Camera CardCamera { get; private set; } = null;
        [field: SerializeField, ReadOnly] public GraphicRaycaster CardRaycaster { get; private set; } = null;
        [field: SerializeField, ReadOnly] public bool IsDragging { get; private set; } = false;
        [field: SerializeField, ReadOnly] public bool OnField { get; set; }
        [field: SerializeField, ReadOnly] public bool IsOverDeckTransform { get; private set; } = false;
        [field: SerializeField, ReadOnly] public bool IsHide { get; set; } = false;

        [field: Header("Input Handler")]
        [field: SerializeField, InitializeRequireComponent]
        public UIInputHandler InputHandler { get; private set; }

        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover CardViewMover { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public Vector2SmoothMover CardBodyMover { get; private set; }
        
        [field: SerializeField, InitializeRequireComponent]
        public RectTransform RectTransform { get; private set; }

        public ITMCardEffect CardEffect { get; private set; } = null;
        public IUnityEventListenerModifier<TMCardModel> OnDragBeginCard => _onDragBeginCard;
        public IUnityEventListenerModifier<TMCardModel> OnDragEndCard => _onDragEndCard;
        public IUnityEventListenerModifier<TMCardModel> OnEffectEvent => _onEffectEvent;

        [SerializeField] private SafeUnityEvent<TMCardModel> _onDragBeginCard = new();
        [SerializeField] private SafeUnityEvent<TMCardModel> _onDragEndCard = new();
        [SerializeField] private SafeUnityEvent<TMCardModel> _onEffectEvent = new();

        [SerializeField, ReadOnly] private bool _isInit = false;

        private bool? _keepIsHide = null;

        private void Awake()
        {
            CardCamera = GameObject
                .FindWithTag("CardCamera")
                .GetComponent<Camera>();
            
            CardRaycaster = GameObject
                .FindWithTag("CardCanvas")
                .GetComponent<GraphicRaycaster>();
        }
        
        public void Initialize()
        {
            if (_isInit) return;

            _isInit = true;
            CardViewMover.IsLocal = true;
            CardBodyMover.IsLocal = true;

            InputHandler.EnterAction.AddListener(eventData
                => CardViewMover.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

            InputHandler.ExitAction.AddListener(eventData
                => CardViewMover.TargetPosition = Vector2.zero);

            InputHandler.DownAction.AddListener(onMouseDownCard);
            InputHandler.DragAction.AddListener(onDrag);
            InputHandler.UpAction.AddListener(onDragEnd);
            
            CardEffect = CardData.CreateCardEffect();
            CardEffect?.ApplyEffect(this, this);
            
            void onMouseDownCard(PointerEventData eventData)
            {
                TriggerSelectCard();
            }
        }

        public void SetMousePosition()
        {
            Vector2 mouseWorldPosition = CardCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
            transform.localPosition = mouseLocalPosition;
        }
        
        public void DragCard()
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager)) return;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(cardManager.DeckTransform, Input.mousePosition, CardCamera))
            {
                if (_keepIsHide is null)
                {
                    _keepIsHide = IsHide;
                    IsOverDeckTransform = true;
                    IsHide = false;
                }
            }
            else
            {
                if (_keepIsHide is not null)
                {
                    IsHide = (bool)_keepIsHide;
                    IsOverDeckTransform = false;
                    _keepIsHide = null;
                }
            }

            SetMousePosition();
        }

        public void TriggerSelectCard()
        {
            setOnMover(CardViewMover, false);
            CardBodyMover.enabled = false;
            IsDragging = true;

            _onDragBeginCard.Invoke(this);
            DragCard();
        }

        private void onDrag(PointerEventData _)
        {
            DragCard();
        }
        
        private void onDragEnd(PointerEventData _)
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager)) return;
                
            Debug.Log("Drag End");
            InputHandler.DragAction.RemoveListener(onDrag);
            InputHandler.UpAction.RemoveListener(onDragEnd);
            IsOverDeckTransform = false;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(cardManager.DeckTransform, Input.mousePosition, CardCamera))
            {
                sellCard();
            }
            else
            {
                _onEffectEvent.Invoke(this);
                setOnMover(CardViewMover, true);
                CardBodyMover.enabled = true;
                IsDragging = false;
            }
            
            _onDragEndCard.Invoke(this);
        }
        
        private static void setOnMover(Vector2SmoothMover smoothMover, bool isOn)
        {
            smoothMover.enabled = isOn;

            if (!isOn)
            {
                smoothMover.transform.localPosition = Vector3.zero;
            }
        }
        
        private void sellCard()
        {
            if (CardEffect is IDisposable disposable)
            {
                disposable.Dispose();
            }
            
            CardEffect = null;
            PlayerManager.Instance.Tera += 10;

            if (ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager))
            {
                cardManager.RemoveCard(this);
            }
            
            Destroy(gameObject);
        }
    }
}