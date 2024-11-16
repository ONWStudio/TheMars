using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Extensions;
using Onw.UI.Components;
using Onw.Components.Movement;
using Onw.Event;
using TM.Card.Effect;

namespace TM.Card.Runtime
{
    public interface ITMCardCostRuntime
    {
        IReactiveField<int> AdditionalCost { get; }
        int FinalCost { get; }
    }

    public interface ITMCardMainCostRuntime : ITMCardCostRuntime
    {
        TMCardMainCost Cost { get; }
    }

    public interface ITMCardSubCostRuntime : ITMCardCostRuntime
    {
        TMCardSubCost Cost { get; }
    }
    
    [Serializable]
    public sealed class TMCardMainCostRuntime : ITMCardMainCostRuntime
    {
        [field: SerializeField] public TMCardMainCost Cost { get; private set; }
        [field: SerializeField] public ReactiveField<int> ReactAdditionalCost { get; private set; } = new();

        public IReactiveField<int> AdditionalCost => ReactAdditionalCost;
        public int FinalCost => Cost.Cost + ReactAdditionalCost.Value;
        
        public TMCardMainCostRuntime(TMCardMainCost cost)
        {
            Cost = cost;
        }
    }

    [Serializable]
    public sealed class TMCardSubCostRuntime : ITMCardSubCostRuntime
    {
        [field: SerializeField] public TMCardSubCost Cost { get; private set; }
        [field: SerializeField] public ReactiveField<int> ReactAdditionalCost { get; private set; } = new();
 
        public IReactiveField<int> AdditionalCost => ReactAdditionalCost;
        public int FinalCost => Cost.Cost + ReactAdditionalCost.Value;
        
        public TMCardSubCostRuntime(TMCardSubCost cost)
        {
            Cost = cost;
        }
    }

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
        [SerializeField, ReadOnly] private TMCardMainCostRuntime _mainCost;
        [SerializeField, ReadOnly] private List<TMCardSubCostRuntime> _subCosts = new();
        [SerializeField, ReadOnly] private bool _isInit = false;

        [Header("Unsafe Input Events")]
        [SerializeField] private UnityEvent<TMCardModel> _onDragEvent = new();

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
        
        public ITMCardEffect CardEffect { get; private set; } = null;

        public bool CanPayCost => _mainCost.FinalCost <= getResourceFromPlayerByMainCost(_mainCost.Cost.CostKind) && 
            _subCosts.All(cost => cost.FinalCost <= getResourceFromPlayerBySubCost(cost.Cost.CostKind));

        public ITMCardMainCostRuntime MainCost => _mainCost;
        public IReadOnlyList<ITMCardSubCostRuntime> SubCosts => _subCosts;

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
            _onSafeDragEvent.AddListener(onDragCard);

            _isInit = true;
            CardViewMover.IsLocal = true;
            CardBodyMover.IsLocal = true;

            void onMouseDownCard(TMCardModel card) => TriggerSelectCard();
        }

        public void TriggerSelectCard()
        {
            setOnMover(CardViewMover, false);
            CardBodyMover.enabled = false;
            _isDragging.Value = true;
            _dragEnumerator = iEOnDrag();
            StartCoroutine(_dragEnumerator);

            _onDragBeginCard.Invoke(this);
            dragCard();
        }

        public void SetCardData(TMCardData cardData)
        {
            _cardData.Value = cardData;
            CardEffect = _cardData.Value.CreateCardEffect();
            CardEffect?.ApplyEffect(this);

            _mainCost = new(_cardData.Value.MainCost);
            _subCosts.AddRange(_cardData.Value.CardCosts.Select(cost => new TMCardSubCostRuntime(cost)));
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
            while (Mouse.current.leftButton.isPressed)
            {
                onDrag(Mouse.current.position.ReadValue());
                yield return null;
            }

            onDragEnd(Mouse.current.position.ReadValue());
        }

        /// <summary>
        /// .. 코스트를 지불합니다 카드를 사용하기전 메서드를 호출해 코스트를 지불해야합니다
        /// 카드 사용 여부와 관계없이 코스트를 지불하는 행동만 수행합니다
        /// </summary>
        private void payCost()
        {
            switch (_mainCost.Cost.CostKind)
            {
                case TMMainCost.CREDIT:
                    TMPlayerManager.Instance.Credit.Value -= _mainCost.FinalCost;
                    break;
                case TMMainCost.ELECTRICITY:
                    TMPlayerManager.Instance.Electricity.Value -= _mainCost.FinalCost;
                    break;
            }
            
            foreach (TMCardSubCostRuntime cost in _subCosts)
            {
                switch (cost.Cost.CostKind)
                {
                    case TMSubCost.MARS_LITHIUM:
                        TMPlayerManager.Instance.MarsLithium.Value -= cost.FinalCost;
                        break;
                    case TMSubCost.STEEL:
                        TMPlayerManager.Instance.Steel.Value -= cost.FinalCost;
                        break;
                    case TMSubCost.PLANTS:
                        TMPlayerManager.Instance.Plants.Value -= cost.FinalCost;
                        break;
                    case TMSubCost.CLAY:
                        TMPlayerManager.Instance.Clay.Value -= cost.FinalCost;
                        break;
                }
            }
        }

        private static int getResourceFromPlayerBySubCost(TMSubCost resourceKind) => resourceKind switch
        {
            TMSubCost.MARS_LITHIUM => TMPlayerManager.Instance.MarsLithium.Value,
            TMSubCost.STEEL => TMPlayerManager.Instance.Steel.Value,
            TMSubCost.PLANTS => TMPlayerManager.Instance.Plants.Value,
            TMSubCost.CLAY => TMPlayerManager.Instance.Clay.Value,
            _ => 0
        };

        private static int getResourceFromPlayerByMainCost(TMMainCost mainCostKind) => mainCostKind switch
        {
            TMMainCost.CREDIT => TMPlayerManager.Instance.Credit.Value,
            TMMainCost.ELECTRICITY => TMPlayerManager.Instance.Electricity.Value,
            _ => 0
        };

        private void onDragCard(TMCardModel card) 
            => dragCard();

        private void onDragEnd(Vector2 mousePosition)
        {
            if (!_canInteract.Value) return;

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
                _isDragging.Value = false;

                if (!_isOverCollectTransform.Value && CanPayCost && CardEffect.CanUseEffect)
                {
                    CardEffect.OnEffect(this);
                    payCost();
                }
            }

            this.StopCoroutineIfNotNull(_dragEnumerator);
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