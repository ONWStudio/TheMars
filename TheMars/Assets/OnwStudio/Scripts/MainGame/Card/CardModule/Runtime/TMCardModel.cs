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
    public interface ICardCostRuntime
    {
        IReactiveField<int> AdditionalCost { get; }
        int FinalCost { get; }
    }
    
    [Serializable]
    public sealed class TMCardMainCostRuntime : ICardCostRuntime
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
    public sealed class TMCardSubCostRuntime : ICardCostRuntime
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
    public sealed class TMCardModel : MonoBehaviour, ITMCardEffectTrigger
    {
        /// <summary>
        /// .. 드래그가 시작될때 호출됩니다
        /// </summary>
        public event UnityAction<TMCardModel> OnDragBeginCard
        {
            add => _onDragBeginCard.AddListener(value);
            remove => _onDragBeginCard.RemoveListener(value);
        }

        /// <summary>
        /// .. 드래그가 끝날때 호출됩니다 
        /// </summary>
        public event UnityAction<TMCardModel> OnDragEndCard
        {
            add => _onDragEndCard.AddListener(value);
            remove => _onDragEndCard.RemoveListener(value);
        }

        /// <summary>
        /// .. 카드가 효과를 발동할때 트리거 됩니다
        /// </summary>
        public event UnityAction<TMCardModel> OnEffectEvent
        {
            add => _onEffectEvent.AddListener(value);
            remove => _onEffectEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnRequestEffectEvent
        {
            add => _onRequestEffectEvent.AddListener(value);
            remove => _onRequestEffectEvent.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnPayCost
        {
            add => _onPayCost.AddListener(value);
            remove => _onPayCost.RemoveListener(value);
        }

        public event UnityAction<TMCardModel> OnSellCard
        {
            add => _onSellCard.AddListener(value);
            remove => _onSellCard.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnSafePointerDownEvent
        {
            add => _onSafePointerDownEvent.AddListener(value);
            remove => _onSafePointerDownEvent.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnSafePointerUpEvent
        {
            add => _onSafePointerUpEvent.AddListener(value);
            remove => _onSafePointerUpEvent.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnSafePointerEnterEvent
        {
            add => _onSafePointerEnterEvent.AddListener(value);
            remove => _onSafePointerEnterEvent.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnSafePointerExitEvent
        {
            add => _onSafePointerExitEvent.AddListener(value);
            remove => _onSafePointerExitEvent.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnSafeDragEvent
        {
            add => _onSafeDragEvent.AddListener(value);
            remove => _onSafeDragEvent.RemoveListener(value);
        }
        
        [Header("Card Events")]
        [SerializeField] private UnityEvent<TMCardModel> _onDragBeginCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onDragEndCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onEffectEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onRequestEffectEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onPayCost = new();
        [SerializeField] private UnityEvent<TMCardModel> _onSellCard = new();

        [Header("Card Options")]
        [SerializeField, ReadOnly] private ReactiveField<TMCardData> _cardData = new() { Value = null };
        [SerializeField, ReadOnly] private ReactiveField<bool> _canInteract = new() { Value = true };
        [SerializeField, ReadOnly] private ReactiveField<bool> _isHide = new();
        [SerializeField, ReadOnly] private TMCardMainCostRuntime _mainCost;
        [SerializeField, ReadOnly] private List<TMCardSubCostRuntime> _subCosts;
        [SerializeField, ReadOnly] private bool _isInit = false;

        [Header("Unsafe Input Events")]
        [SerializeField] private UnityEvent<Vector2> _onDragEvent = new();

        [Header("Safe Input Events")]
        [SerializeField] private UnityEvent<Vector2> _onSafePointerDownEvent = new();
        [SerializeField] private UnityEvent<Vector2> _onSafePointerUpEvent = new();
        [SerializeField] private UnityEvent<Vector2> _onSafePointerEnterEvent = new();
        [SerializeField] private UnityEvent<Vector2> _onSafePointerExitEvent = new();
        [SerializeField] private UnityEvent<Vector2> _onSafeDragEvent = new();

        private bool? _keepIsHide = null;
        private IEnumerator _dragEnumerator = null;
        public IReactiveField<TMCardData> CardData => _cardData;

        public void SetCardData(TMCardData cardData)
        {
            _cardData.Value = cardData;
            CardEffect = _cardData.Value.CreateCardEffect();
            CardEffect?.ApplyEffect(this, this);

            _mainCost = new(_cardData.Value.MainCost);
            _subCosts = new(_cardData.Value.CardCosts.Select(cost => new TMCardSubCostRuntime(cost)));
        }
        
        /// <summary>
        /// .. 카드가 출력되는 카메라 입니다
        /// </summary>
        [field: Header("Runtime Option")]
        [field: SerializeField, ReadOnly] public Camera CardCamera { get; private set; } = null;

        
        /// <summary>
        /// .. 카드 위에 마우스 포인터 등이 올라올때등의 하이라이트 효과를 위한 컴포넌트입니다
        /// </summary>
        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover CardViewMover { get; private set; }

        /// <summary>
        /// .. 카드의 움직임을 담당하는 무버 컴포넌트입니다
        /// </summary>
        [field: SerializeField, InitializeRequireComponent]
        public Vector2SmoothMover CardBodyMover { get; private set; }

        /// <summary>
        /// .. 카드의 렉트 트랜스폼입니다
        /// </summary>
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
        
        /// <summary>
        /// .. 카드의 효과 인터페이스를 제공합니다 CardData에서 정보를 받아와 런타임에 이펙트를 동적으로 생성합니다
        /// </summary>
        public ITMCardEffect CardEffect { get; private set; } = null;

        /// <summary>
        /// .. 카드의 코스트를 지불 가능한 상태를 반환합니다
        /// </summary>
        public bool CanPayCost => _mainCost.FinalCost <= getResourceFromPlayerByMainCost(_mainCost.Cost.CostKind) && _subCosts
            .All(cost => cost.FinalCost <= getResourceFromPlayerBySubCost(cost.Cost.CostKind));

        public ICardCostRuntime MainCost => _mainCost;
        public IReadOnlyList<ICardCostRuntime> SubCosts => _subCosts;

        public IReadOnlyReactiveField<bool> CanInteract => _canInteract;
        public IReactiveField<bool> IsHide => _isHide;
        public IReadOnlyReactiveField<bool> IsDragging => _isDragging;
        public IReadOnlyReactiveField<bool> OnField => _onField;
        public IReadOnlyReactiveField<bool> IsOverTombTransform => _isOverTombTransform;


        [SerializeField, ReadOnly] private ReactiveField<bool> _isDragging = new();
        [SerializeField, ReadOnly] private ReactiveField<bool> _onField = new();
        [SerializeField, ReadOnly] private ReactiveField<bool> _isOverTombTransform = new();

        public void SetCanInteract(bool canInteract)
        {
            CardBodyMover.enabled = canInteract;
            CardViewMover.enabled = canInteract;
            _canInteract.Value = canInteract;
        }

        private void Awake()
        {
            CardCamera = GameObject
                .FindWithTag("CardCamera")
                .GetComponent<Camera>();
        }

        private void onPointerDown(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerDownEvent.Invoke(pointerEventData.position);
        }

        private void onPointerUp(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerUpEvent.Invoke(pointerEventData.position);
        }

        private void onPointerEnter(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerEnterEvent.Invoke(pointerEventData.position);
        }

        private void onPointerExit(PointerEventData pointerEventData)
        {
            if (!_canInteract.Value) return;

            _onSafePointerExitEvent.Invoke(pointerEventData.position);
        }

        private void onDrag(Vector2 mousePosition)
        {
            if (!_canInteract.Value) return;

            _onSafeDragEvent.Invoke(mousePosition);
        }

        /// <summary>
        /// .. 카드의 초기화 메서드 입니다 한번 초기화를 수행한 후 이후 다시 초기화를 시도할 경우 초기화가 되지 않습니다 최초의 호출 한번만 수행합니다
        /// </summary>
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

            void onMouseDownCard(Vector2 mousePosition) => TriggerSelectCard();
        }

        /// <summary>
        /// .. 현재 카드 위치를 스크린 상의 마우스 위치로 이동시킵니다 CardBodyMover가 활성화 되어 있을 경우 카드의 움직임이 부자연스러울 수 있습니다
        /// </summary>
        public void SetMousePosition()
        {
            Vector2 mouseWorldPosition = CardCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
            transform.localPosition = mouseLocalPosition;
        }

        private void dragCard()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(TMCardManager.Instance.DeckTransform, Input.mousePosition, CardCamera))
            {
                if (_keepIsHide is null)
                {
                    _keepIsHide = IsHide.Value;
                    _isOverTombTransform.Value = true;
                    IsHide.Value = false;
                }
            }
            else
            {
                if (_keepIsHide is not null)
                {
                    IsHide.Value = (bool)_keepIsHide;
                    _isOverTombTransform.Value = false;
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
        /// .. 카드가 드래그 상태로 전환될때 설정되어야할 변수나 메서드를 호출합니다
        /// </summary>
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

        /// <summary>
        /// .. 코스트를 지불합니다 카드를 사용하기전 메서드를 호출해 코스트를 지불해야합니다
        /// 카드 사용 여부와 관계없이 코스트를 지불하는 행동만 수행합니다
        /// </summary>
        public void PayCost()
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

            _onPayCost.Invoke(this);
        }

        /// <summary>
        /// .. 코스트 종류에 따른 현재 플레이어의 자원을 리턴합니다
        /// </summary>
        /// <param name="resourceKind"></param>
        /// <returns></returns>
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

        private void onDragCard(Vector2 mousePosition) => dragCard();

        private void onDragEnd(Vector2 mousePosition)
        {
            if (!_canInteract.Value) return;

            _isOverTombTransform.Value = false;

            if (RectTransformUtility.RectangleContainsScreenPoint(TMCardManager.Instance.DeckTransform, mousePosition, CardCamera))
            {
                sellCard();
            }
            else
            {
                setOnMover(CardViewMover, true);
                CardBodyMover.enabled = true;
                _isDragging.Value = false;
                _onEffectEvent.Invoke(this);
            }

            this.StopCoroutineIfNotNull(_dragEnumerator);
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
            _onSellCard.Invoke(this);
            CardEffect.Is<IDisposable>(disposable => disposable.Dispose());
            CardEffect = null;
            Destroy(gameObject);
        }
    }
}