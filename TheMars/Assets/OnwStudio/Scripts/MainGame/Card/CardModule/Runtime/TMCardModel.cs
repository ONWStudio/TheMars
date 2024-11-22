using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Onw.Event;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Extensions;
using Onw.UI.Components;
using Onw.Components.Movement;
using TM.Card.Effect;
using TM.Event;
using TM.Cost;

namespace TM.Card.Runtime
{
    // .. Model
    [DisallowMultipleComponent]
    public sealed class TMCardModel : MonoBehaviour
    {
        public event UnityAction<TMCardModel> OnDragBeginCard
        {
            add => _onDragBeginCard.AddListener(value);
            remove => _onDragBeginCard.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnDragEndCard
        {
            add => _onDragEndCard.AddListener(value);
            remove => _onDragEndCard.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSellCard
        {
            add => _onSellCard.AddListener(value);
            remove => _onSellCard.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSafePointerDownEvent
        {
            add => _onSafePointerDownEvent.AddListener(value);
            remove => _onSafePointerDownEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSafePointerUpEvent
        {
            add => _onSafePointerUpEvent.AddListener(value);
            remove => _onSafePointerUpEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSafePointerEnterEvent
        {
            add => _onSafePointerEnterEvent.AddListener(value);
            remove => _onSafePointerEnterEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSafePointerExitEvent
        {
            add => _onSafePointerExitEvent.AddListener(value);
            remove => _onSafePointerExitEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSafeDragEvent
        {
            add => _onSafeDragEvent.AddListener(value);
            remove => _onSafeDragEvent.RemoveListener(value);
        }

        [SerializeField, ReadOnly] private ReactiveField<bool> _isDragging = new();
        [SerializeField, ReadOnly] private ReactiveField<bool> _onField = new();
        [SerializeField, ReadOnly] private ReactiveField<bool> _isOverTombTransform = new();
        [SerializeField, ReadOnly] private ReactiveField<bool> _isOverCollectTransform = new();

        [Header("Card Events")]
        [SerializeField] private UnityEvent<TMCardModel> _onDragBeginCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onDragEndCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onSellCard = new();

        [Header("Card Options")]
        [SerializeField, ReadOnly] private ReactiveField<TMCardData> _cardData = new() { Value = null };
        [SerializeField, ReadOnly] private ReactiveField<bool> _canInteract = new() { Value = true };
        [SerializeField, ReadOnly] private ReactiveField<bool> _isHide = new();
        [SerializeReference, ReadOnly] private List<ITMResourceCost> _subCosts = new();
        [SerializeField, ReadOnly] private bool _isInit = false;

        [Header("Safe Input Events")]
        [SerializeField] private UnityEvent<TMCardModel> _onSafePointerDownEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onSafePointerUpEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onSafePointerEnterEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onSafePointerExitEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onSafeDragEvent = new();

        private bool? _keepIsHide = null;
        private IEnumerator _dragEnumerator = null;
        public IReadOnlyReactiveField<TMCardData> CardData => _cardData;

        [field: Header("Runtime Option")]
        [field: SerializeField, ReadOnly] public Camera CardCamera { get; private set; } = null;
        
        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover CardViewMover { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public Vector2SmoothMover CardBodyMover { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public RectTransform RectTransform { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public PointerEnterTrigger PointerEnterTrigger { get; private set; }
        
        [field: SerializeField, InitializeRequireComponent]
        public PointerExitTrigger PointerExitTrigger { get; private set; }
        
        [field: SerializeField, InitializeRequireComponent]
        public PointerDownTrigger PointerDownTrigger { get; private set; }
        
        [field: SerializeField, InitializeRequireComponent]
        public PointerUpTrigger PointerUpTrigger { get; private set; }
        
        [field: SerializeReference, ReadOnly] public ITMCardEffect CardEffect { get; private set; } = null;

        public bool CanPayCost => MainCost.FinalCost <= TMPlayerManager.Instance.GetResourceByKind(MainCost.Kind) && 
            _subCosts.All(cost => cost.FinalCost <= TMPlayerManager.Instance.GetResourceByKind(cost.Kind));

        [field: SerializeReference, ReadOnly] public ITMResourceCost MainCost { get; private set; }

        public IReadOnlyList<ITMResourceCost> SubCosts => _subCosts;

        public IReadOnlyReactiveField<bool> CanInteract => _canInteract;
        public IReactiveField<bool> IsHide => _isHide;
        public IReadOnlyReactiveField<bool> IsDragging => _isDragging;
        public IReadOnlyReactiveField<bool> OnField => _onField;
        public IReadOnlyReactiveField<bool> IsOverTombTransform => _isOverTombTransform;
        public IReadOnlyReactiveField<bool> IsOverCollectTransform => _isOverCollectTransform;

        private void Awake()
        {
            CardCamera = GameObject
                .FindWithTag("CardCamera")
                .GetComponent<Camera>();
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;

            CardEffect.Is<IDisposable>(disposable => disposable.Dispose());
            CardEffect = null;
        }

        public void Initialize()
        {
            if (_isInit) return;

            PointerDownTrigger.OnPointerDownEvent += onPointerDown;
            PointerUpTrigger.OnPointerUpEvent += onPointerUp;
            PointerEnterTrigger.OnPointerEnterEvent += onPointerEnter;
            PointerExitTrigger.OnPointerExitEvent += onPointerExit;

            _onSafePointerEnterEvent.AddListener(_ => CardViewMover.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));
            _onSafePointerExitEvent.AddListener(_ => CardViewMover.TargetPosition = Vector2.zero);
            _onSafePointerDownEvent.AddListener(onMouseDownCard);

            _isInit = true;
            CardViewMover.IsLocal = true;
            CardBodyMover.IsLocal = true;

            void onMouseDownCard(TMCardModel card) => TriggerSelectCard();
        }

        public void TriggerSelectCard()
        {
            setOnMover(CardViewMover, false);
            CardBodyMover.enabled = false;
            _dragEnumerator = iEOnDrag();
            StartCoroutine(_dragEnumerator);
            dragCard();

            _onDragBeginCard.Invoke(this);
            _isDragging.Value = true;
        }

        public void SetCardData(TMCardData cardData)
        {
            _cardData.Value = cardData;
            CardEffect = cardData.CreateCardEffect();
            CardEffect?.ApplyEffect(this);

            MainCost = cardData.CreateMainCost();
            _subCosts.AddRange(cardData.CreateSubCosts());
        }

        public void SetCanInteract(bool canInteract)
        {
            CardBodyMover.enabled = canInteract;
            CardViewMover.enabled = canInteract;
            _canInteract.Value = canInteract;
        }

        public void SetMousePosition()
        {
            Vector2 mouseWorldPosition = CardCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
            transform.localPosition = mouseLocalPosition;
        }

        private void onPointerDown(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerDownEvent.Invoke(this);
        }

        private void onPointerUp(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerUpEvent.Invoke(this);
        }

        private void onPointerEnter(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerEnterEvent.Invoke(this);
        }

        private void onPointerExit(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerExitEvent.Invoke(this);
        }

        private void onDrag(Vector2 mousePosition)
        {
            if (!_canInteract.Value) return;

            _onSafeDragEvent.Invoke(this);
        }

        private void dragCard()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            _isOverTombTransform.Value = RectTransformUtility.RectangleContainsScreenPoint(
                TMCardManager.Instance.DeckTransform,
                mousePosition,
                CardCamera);

            _isOverCollectTransform.Value = RectTransformUtility.RectangleContainsScreenPoint(
                TMCardManager.Instance.UIComponents.CollectField,
                mousePosition,
                CardCamera);

            if (_isOverTombTransform.Value || _isOverCollectTransform.Value)
            {
                if (_keepIsHide is null)
                {
                    _keepIsHide = IsHide.Value;
                    IsHide.Value = false;
                }
            }
            else
            {
                if (_keepIsHide is not null)
                {
                    IsHide.Value = (bool)_keepIsHide;
                    _keepIsHide = null;
                }
            }

            SetMousePosition();
        }

        private IEnumerator iEOnDrag()
        {
            Mouse current = Mouse.current;
            Vector2 mousePosition = current.position.ReadValue();

            bool isCancel = false;
            TMEventManager.Instance.OnTriggerEvent += onTriggerEvent;

            while (current.leftButton.isPressed && !isCancel)
            {
                dragCard();
                onDrag(mousePosition);

                yield return null;
            }

            onDragEnd(Mouse.current.position.ReadValue(), isCancel);

            void onTriggerEvent(ITMEventRunner eventRunner)
            {
                TMEventManager.Instance.OnTriggerEvent -= onTriggerEvent;
                isCancel = true;
            }
        }

        /// <summary>
        /// .. 코스트를 지불합니다 카드를 사용하기전 메서드를 호출해 코스트를 지불해야합니다
        /// 카드 사용 여부와 관계없이 코스트를 지불하는 행동만 수행합니다
        /// </summary>
        private void payCost()
        {
            TMPlayerManager.Instance.AddResource(MainCost.Kind, -MainCost.FinalCost);
            _subCosts.ForEach(subCost => TMPlayerManager.Instance.AddResource(subCost.Kind, -subCost.FinalCost));
        }

        private void onDragEnd(Vector2 mousePosition, bool isCancel)
        {
            if (!_canInteract.Value) return;

            if (!isCancel)
            {
                _isOverTombTransform.Value = RectTransformUtility.RectangleContainsScreenPoint(
                    TMCardManager.Instance.DeckTransform,
                    mousePosition,
                    CardCamera);

                _isOverCollectTransform.Value = RectTransformUtility.RectangleContainsScreenPoint(
                    TMCardManager.Instance.UIComponents.CollectField,
                    mousePosition,
                    CardCamera);

                if (_isOverTombTransform.Value)
                {
                    sellCard();
                }
                else
                {
                    setOnMover(CardViewMover, true);
                    CardBodyMover.enabled = true;

                    if (!_isOverCollectTransform.Value && CanPayCost && CardEffect.CanUseEffect)
                    {
                        CardEffect.OnEffect(this);
                        payCost();
                    }
                }
            }

            this.StopCoroutineIfNotNull(_dragEnumerator);
            _isDragging.Value = false;
            _onDragEndCard.Invoke(this);
            _isOverTombTransform.Value = false;
            _isOverCollectTransform.Value = false;
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
            _onSellCard.Invoke(this);

            Destroy(gameObject);
        }
    }
}