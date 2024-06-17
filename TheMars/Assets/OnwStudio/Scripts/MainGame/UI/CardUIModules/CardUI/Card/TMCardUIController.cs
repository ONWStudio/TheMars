using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

namespace TMCardUISystemModules
{
    [DisallowMultipleComponent]
    public sealed class TMCardUIController : MonoBehaviour, ITMCardController<TMCardUIController>
    {
        /// <summary>
        /// .. 카드를 사용할때 (연출효과) 호출되는 콜백 이벤트
        /// </summary>
        public UnityEvent<TMCardUIController> OnUseStarted => _onUseStarted;
        /// <summary>
        /// .. 카드의 사용 후 (연출효과) 호출되는 콜백 이벤트 
        /// </summary>
        public UnityEvent<TMCardUIController> OnUseEnded => _onUseEnded;
        /// .. 카드가 무덤으로 이동해야 되는 경우 리스너들에게 알려주는 콜백 이벤트
        /// 카드 UI에서는 무덤으로 이동 할 방법을 알지못하기 때문에 무덤의 위치정보를 가지고 있는 객체가 카드 UI를 참조해서 알려주어야 함
        /// </summary>
        public UnityEvent<TMCardUIController> OnMoveToTomb => _onMoveToTomb;
        /// <summary>
        /// .. 카드가 사용 후 다시 패로 돌아가야 하는 경우 리스너들에게 알려주는 콜백 이벤트 입니다
        /// </summary>
        public UnityEvent<TMCardUIController> OnRecycleToHand => _onRecycleToHand;
        /// <summary>
        /// .. 카드가 드로우 카드 인 경우 드로우 시 사용됐을때 리스너들에게 알려주는 콜백 이벤트 입니다
        /// </summary>
        public UnityEvent<TMCardUIController> OnDrawUse => _onDrawUse;
        /// <summary>
        /// .. 카드가 파괴되는 경우 리스너들에게 알려줍니다
        /// </summary>
        public UnityEvent<TMCardUIController> OnDestroyCard => _onDestroyCard;
        /// <summary>
        /// .. 카드가 소요일 경우 리스너들에게 알려줍니다 시간
        /// </summary>
        public UnityEvent<TMCardUIController, float> OnDelaySeconds => _onDelaySeconds;
        /// <summary>
        /// .. 카드가 소요일 경우 리스너들에게 알려줍니다 턴
        /// </summary>
        public UnityEvent<TMCardUIController, int> OnDelayTurn => _onDelayTurn;
        /// <summary>
        /// .. 카드가 보유일 경우 리스너들에게 알려줍니다
        /// </summary>
        public UnityEvent<TMCardUIController, int> OnHoldCard => _onHoldCard;

        /// <summary>
        /// .. 이벤트 센더 클래스 입니다 외부에서 이벤트 연출 효과를 발생시킬때 사용할 수 있는 프로퍼티 입니다
        /// </summary>
        public EventSender EventSender { get; private set; } = null;
        /// <summary>
        /// .. 카드에 사용자 입력을 받아오는 핸들러 입니다
        /// </summary>
        public TMCardInputHandler InputHandler { get; private set; } = null;
        /// <summary>
        /// .. 카드의 상세한 기본 데이터 입니다
        /// </summary>
        public RectTransform RectTransform { get; private set; } = null;
        public TMCardUIController Follower { get; set; } = null;
        public TMCardData CardData => _cardData;

        /// <summary>
        /// .. 핸들러에서 마우스 포인터가 카드 위에 존재할 시 카드가 선택된 듯이 보이게 하기 위한 Mover 클래스 입니다
        /// </summary>
        ///

        [SerializeField] private UnityEvent<TMCardUIController> _onUseStarted = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onUseEnded = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onMoveToTomb = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onRecycleToHand = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onDrawUse = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onDestroyCard = new();
        [SerializeField] private UnityEvent<TMCardUIController, float> _onDelaySeconds = new();
        [SerializeField] private UnityEvent<TMCardUIController, int> _onDelayTurn = new();
        [SerializeField] private UnityEvent<TMCardUIController, int> _onHoldCard = new();

        private TMCardData _cardData = null;
        private SmoothMoveVector2 _smoothMove = null;
        private Image _raycastingImage = null;
        private Image _cardImage = null;

        public bool OnCard { get; private set; } = false;

        private void Awake()
        {
            RectTransform = transform as RectTransform;
            EventSender = gameObject.AddComponent<EventSender>();
            InputHandler = gameObject.AddComponent<TMCardInputHandler>();
            _raycastingImage = gameObject.AddComponent<Image>();
            _cardImage = new GameObject("CardImage").AddComponent<Image>();
            _smoothMove = _cardImage.gameObject.AddComponent<SmoothMoveVector2>();
        }

        private void Start()
        {
            initializeImages();
            initalizeInputHandle();
            initializeSmoothMove();
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            Debug.Log("destroyed Card!");
#endif

            _onDestroyCard.Invoke(this);
        }

        private void initializeImages()
        {
            _raycastingImage.color = new(255f, 255f, 255f, 0f);

            _cardImage.transform.SetParent(transform, false);
            _cardImage.raycastTarget = false;
            _cardImage.transform.localPosition = Vector3.zero;
        }

        private void initializeSmoothMove()
        {
            _smoothMove.IsLocal = true;

            if (!OnCard)
            {
                _smoothMove.enabled = false;
            }
        }

        private void initalizeInputHandle()
        {
            InputHandler.AddListenerPointerEnterAction(pointerEventData
                 => _smoothMove.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

            InputHandler.AddListenerPointerExitAction(pointerEventData
                => _smoothMove.TargetPosition = Vector2.zero);

            InputHandler.AddListenerPointerClickAction(onClickCard);
        }

        /// <summary>
        /// .. 카드를 사용합니다
        /// 카드 사용시 카드는 효과가 발동되며 카드 효과는 이벤트 기반입니다 
        /// </summary>
        /// <param name="pointerEventData"></param>
        private void onClickCard(PointerEventData pointerEventData)
        {
            if (!OnCard) return;

            UseCard();
        }

        public void UseCard()
        {
            _cardData.StateMachine.OnUseStarted(this);
        }

        /// <summary>
        /// .. 카드가 덱에서 드로우 될 때 호출되는 메서드 입니다
        /// </summary>
        public void OnDrawBegin()
        {
            _cardData.StateMachine.OnDrawBegin(this);
        }

        public void OnDrawEnded()
        {
            _cardData.StateMachine.OnDrawEnded(this);
        }

        /// <summary>
        /// .. 턴이 종료되었을때 TurnEnd메서드를 호출합니다
        /// </summary>
        public void OnTurnEnd()
        {
            _cardData.StateMachine.OnTurnEnd(this);
        }

        /// <summary>
        /// .. 카드의 상호작용 상태를 전환 하는 메서드 입니다 false 일시 카드와 상호작용이 불가능 합니다
        /// </summary>
        /// <param name="isOn"> .. 카드의 상호작용 상태를 전환시킬 boolen 값 </param>
        public void SetOn(bool isOn)
        {
            OnCard = isOn;
            _smoothMove.enabled = isOn;
            _smoothMove.transform.localPosition = Vector3.zero;
        }
    }
}
