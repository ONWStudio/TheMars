using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Onw.Attribute;
using Onw.UI.Components;
using Onw.ServiceLocator;
using Onw.Components.Movement;
using TM.Card.Effect;
using UnityEngine.Serialization;

namespace TM.Card.Runtime
{
    // .. Model
    [DisallowMultipleComponent]
    public sealed class TMCardModel : MonoBehaviour, ITMCardEffectTrigger
    {
        /// <summary>
        /// .. 카드에 필요한 정보등을 보유한 스크립터블 오브젝트입니다
        /// </summary>
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; }

        /// <summary>
        /// .. 카드가 출력되는 카메라 입니다
        /// </summary>
        [field: Header("Runtime Option")]
        [field: SerializeField, ReadOnly] public Camera CardCamera { get; private set; } = null;
        
        /// <summary>
        /// .. 현재가 카드가 드래그를 수행중인지에 대한 여부를 반환합니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool IsDragging { get; private set; } = false;
        /// <summary>
        /// .. 현재 카드가 패위에 있는 지에 대한 여부를 반환합니다 외부에서 수정될 수 있습니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool OnField { get; set; }
        /// <summary>
        /// .. 현재 카드가 버리기 칸위에 오버랩되고 있는지에 대한 여부를 반환합니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool IsOverTombTransform { get; private set; } = false;
        /// <summary>
        /// .. 카드의 View상태를 반환합니다 해당 값이 true일 경우 카드가 보이지 않게 됩니다 외부에서 수정될 수 있습니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool IsHide { get; set; } = false;
        
        /// <summary>
        /// .. 현재 카드가 코스트를 이미 지불하였는지에 대한 여부를 반환합니다 외부에서 수정될 수 있습니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool WasCostPaid { get; set; } = false;

        /// <summary>
        /// .. 카드의 클릭, 마우스 다운, 마우스 업, 드래그 등에 관한 이벤트 메서드를 제공하는 핸들러 컴포넌트입니다
        /// </summary>
        [field: Header("Input Handler")]
        [field: SerializeField, InitializeRequireComponent]
        public UIInputHandler InputHandler { get; private set; }

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

        /// <summary>
        /// .. 카드의 효과 인터페이스를 제공합니다 CardData에서 정보를 받아와 런타임에 이펙트를 동적으로 생성합니다
        /// </summary>
        public ITMCardEffect CardEffect { get; private set; } = null;

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
        
        public event UnityAction<TMCardModel> OnPayCost
        {
            add => _onPayCost.AddListener(value);
            remove => _onPayCost.RemoveListener(value);
        }

        /// <summary>
        /// .. 카드의 코스트를 지불 가능한 상태를 반환합니다
        /// </summary>
        public bool CanPayCost => CardData
            .CardCosts
            .All(cardCost => cardCost.Cost <= getResourceFromPlayerByCost(cardCost.ResourceKind));
        
        [SerializeField] private UnityEvent<TMCardModel> _onDragBeginCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onDragEndCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onEffectEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onPayCost = new();

        [SerializeField, ReadOnly] private bool _isInit = false;

        private bool? _keepIsHide = null;

        private void Awake()
        {
            CardCamera = GameObject
                .FindWithTag("CardCamera")
                .GetComponent<Camera>();
        }
        
        /// <summary>
        /// .. 카드의 초기화 메서드 입니다 한번 초기화를 수행한 후 이후 다시 초기화를 시도할 경우 초기화가 되지 않습니다 최초의 호출 한번만 수행합니다
        /// </summary>
        public void Initialize()
        {
            if (_isInit) return;

            _isInit = true;
            CardViewMover.IsLocal = true;
            CardBodyMover.IsLocal = true;

            InputHandler.EnterAction += _ => CardViewMover.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f);
            InputHandler.ExitAction += _ => CardViewMover.TargetPosition = Vector2.zero;
            InputHandler.DownAction += onMouseDownCard;
            InputHandler.DragAction += onDrag;
            InputHandler.UpAction += onDragEnd;
            
            CardEffect = CardData.CreateCardEffect();
            CardEffect?.ApplyEffect(this, this);
            
            void onMouseDownCard(PointerEventData eventData)
            {
                TriggerSelectCard();
            }
        }

        /// <summary>
        /// .. 현재 카드 위치를 스크린 상의 마우스 위치로 이동시킵니다 CardBodyMover가 활성화 되어 있을 경우 카드의 움직임이 부자연스러울 수 있습니다
        /// </summary>
        public void SetMousePosition()
        {
            Vector2 mouseWorldPosition = CardCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
            transform.localPosition = mouseLocalPosition;
        }
        
        private void dragCard()
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager)) return;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(cardManager.DeckTransform, Input.mousePosition, CardCamera))
            {
                if (_keepIsHide is null)
                {
                    _keepIsHide = IsHide;
                    IsOverTombTransform = true;
                    IsHide = false;
                }
            }
            else
            {
                if (_keepIsHide is not null)
                {
                    IsHide = (bool)_keepIsHide;
                    IsOverTombTransform = false;
                    _keepIsHide = null;
                }
            }

            SetMousePosition();
        }

        /// <summary>
        /// .. 카드가 드래그 상태로 전환될때 설정되어야할 변수나 메서드를 호출합니다
        /// </summary>
        public void TriggerSelectCard()
        {
            setOnMover(CardViewMover, false);
            CardBodyMover.enabled = false;
            IsDragging = true;

            _onDragBeginCard.Invoke(this);
            dragCard();
        }

        /// <summary>
        /// .. 코스트를 지불합니다 카드를 사용하기전 메서드를 호출해 코스트를 지불해야합니다
        /// 카드 사용 여부와 관계없이 코스트를 지불하는 행동만 수행합니다
        /// </summary>
        public void PayCost()
        {
            if (!ServiceLocator<PlayerManager>.TryGetService(out PlayerManager player)) return;
            
            foreach (TMCardCost cost in CardData.CardCosts)
            {
                switch (cost.ResourceKind)
                {
                    case TMResourceKind.MARS_LITHIUM:
                        player.MarsLithium -= cost.Cost;
                        break;
                    case TMResourceKind.CREDIT:
                        player.Credit -= cost.Cost;
                        break;
                    case TMResourceKind.STEEL:
                        player.Steel -= cost.Cost;
                        break;
                    case TMResourceKind.PLANTS:
                        player.Plants -= cost.Cost;
                        break;
                    case TMResourceKind.CLAY:
                        player.Clay -= cost.Cost;
                        break;
                    case TMResourceKind.ELECTRICITY:
                        player.Electricity -= cost.Cost;
                        break;
                    case TMResourceKind.POPULATION:
                        player.Population -= cost.Cost;
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
        private static int getResourceFromPlayerByCost(TMResourceKind resourceKind)
        {
            if (!ServiceLocator<PlayerManager>.TryGetService(out PlayerManager player)) return 0;

            return resourceKind switch
            {
                TMResourceKind.MARS_LITHIUM => player.MarsLithium,
                TMResourceKind.CREDIT => player.Credit,
                TMResourceKind.STEEL => player.Steel,
                TMResourceKind.PLANTS => player.Plants,
                TMResourceKind.CLAY => player.Clay,
                TMResourceKind.POPULATION => player.Population,
                TMResourceKind.ELECTRICITY => player.Electricity,
                _ => 0
            };
        }

        private void onDrag(PointerEventData _)
        {
            dragCard();
        }
        
        private void onDragEnd(PointerEventData _)
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager)) return;

            IsOverTombTransform = false;
            
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
            ServiceLocator<PlayerManager>.InvokeService(player => player.Credit += 10);
            ServiceLocator<TMCardManager>.InvokeService(cardManager => cardManager.RemoveCard(this));
            Destroy(gameObject);
        }
    }
}