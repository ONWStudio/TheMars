using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        [field: Header("State")]
        [field: SerializeField, ReadOnly] public bool IsDragging { get; private set; } = false;
        [field: SerializeField, ReadOnly] public bool OnField { get; set; }

        [field: SerializeField, InitializeRequireComponent]
        public RectTransform RectTransform { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public UIInputHandler InputHandler { get; private set; }

        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover CardViewMover { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public Vector2SmoothMover CardBodyMover { get; private set; }

        [field: SerializeField, ReadOnly] public bool IsOverDeckTransform { get; private set; } = false;
        [field: SerializeField, ReadOnly] public bool IsHide { get; set; } = false;

        public ITMCardEffect CardEffect { get; private set; } = null;
        public IUnityEventListenerModifier OnDragBeginCard => _onDragBeginCard;
        public IUnityEventListenerModifier OnDragEndCard => _onDragEndCard;
        public IUnityEventListenerModifier OnEffectEvent => _onEffectEvent;

        [SerializeField] private SafeUnityEvent _onDragBeginCard = new();
        [SerializeField] private SafeUnityEvent _onDragEndCard = new();
        [SerializeField] private SafeUnityEvent _onEffectEvent = new();

        [SerializeField, ReadOnly] private bool _isInit = false;

        public void OnMouseDownCard(PointerEventData eventData)
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager)) return;

            Camera cardSystemCamera = eventData.enterEventCamera;
            bool? keepIsHide = null;

            setOnMover(CardViewMover, false);
            CardBodyMover.enabled = false;

            InputHandler.DragAction.AddListener(onDrag);
            InputHandler.UpAction.AddListener(onDragEnd);
            IsDragging = true;

            _onDragBeginCard.Invoke();
            
            void onDrag(PointerEventData dragEventData)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cardManager.DeckTransform, Input.mousePosition, cardSystemCamera))
                {
                    if (keepIsHide is null)
                    {
                        keepIsHide = IsHide;
                        IsOverDeckTransform = true;
                        IsHide = false;
                    }
                }
                else
                {
                    if (keepIsHide is not null)
                    {
                        IsHide = (bool)keepIsHide;
                        IsOverDeckTransform = false;
                        keepIsHide = null;
                    }
                }
                
                Vector2 mouseWorldPosition = cardSystemCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
                transform.localPosition = mouseLocalPosition;
            }

            void onDragEnd(PointerEventData dragEndEventData)
            {
                InputHandler.DragAction.RemoveListener(onDrag);
                InputHandler.UpAction.RemoveListener(onDragEnd);
                IsOverDeckTransform = false;
                if (RectTransformUtility.RectangleContainsScreenPoint(cardManager.DeckTransform, Input.mousePosition, cardSystemCamera))
                {
                    sellCard();
                }
                else
                {
                    _onEffectEvent.Invoke();
                    setOnMover(CardViewMover, true);
                    CardBodyMover.enabled = true;
                    IsDragging = false;
                }

                _onDragEndCard.Invoke();
            }

            static void setOnMover(Vector2SmoothMover smoothMover, bool isOn)
            {
                smoothMover.enabled = isOn;

                if (!isOn)
                {
                    smoothMover.transform.localPosition = Vector3.zero;
                }
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
            Destroy(gameObject);
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

            InputHandler.DownAction.AddListener(OnMouseDownCard);
            
            CardEffect = CardData.GetCardEffect();
            CardEffect?.ApplyEffect(this, this);
        }
    }
}