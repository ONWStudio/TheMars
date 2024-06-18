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
        public UnityEvent<TMCardUIController> OnUseStarted => _onUseStarted;
        public UnityEvent<TMCardUIController> OnUseEnded => _onUseEnded;
        public UnityEvent<TMCardUIController> OnMoveToTomb => _onMoveToTomb;
        public UnityEvent<TMCardUIController> OnRecycleToHand => _onRecycleToHand;
        public UnityEvent<TMCardUIController> OnDrawUse => _onDrawUse;
        public UnityEvent<TMCardUIController> OnDestroyCard => _onDestroyCard;
        public UnityEvent<TMCardUIController, float> OnDelaySeconds => _onDelaySeconds;
        public UnityEvent<TMCardUIController, string> OnHoldCard => _onHoldCard;
        public UnityEvent<TMCardUIController, int> OnDelayTurn => _onDelayTurn;

        /// <summary>
        /// .. 이벤트 센더 클래스 입니다 외부에서 이벤트 연출 효과를 발생시킬때 사용할 수 있는 프로퍼티 입니다
        /// </summary>
        public EventSender EventSender { get; private set; } = null;
        /// <summary>
        /// .. 카드에 사용자 입력을 받아오는 핸들러 입니다
        /// </summary>
        public TMCardInputHandler InputHandler { get; private set; } = null;
        /// <summary>
        /// .. RectTransform
        /// </summary>
        public RectTransform RectTransform { get; private set; } = null;
        /// <summary>
        /// .. 보유인 카드가 존재할 경우 해당 카드를 팔로우중일수 있습니다. 팔로워가 있다면 카드를 사용후 팔로워의 카드효과도 발동됩니다
        /// </summary>
        public TMCardUIController Follower { get; set; } = null;
        /// <summary>
        /// .. 카드가 상호작용 가능한 활성화인지 상태를 반환합니다
        /// </summary>
        public bool OnCard { get; private set; } = false;
        /// <summary>
        /// .. 카드의 상세한 기본 데이터 입니다
        /// </summary>
        public TMCardData CardData => _cardData;

        [SerializeField] private UnityEvent<TMCardUIController> _onUseStarted = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onUseEnded = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onMoveToTomb = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onRecycleToHand = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onDrawUse = new();
        [SerializeField] private UnityEvent<TMCardUIController> _onDestroyCard = new();
        [SerializeField] private UnityEvent<TMCardUIController, float> _onDelaySeconds = new();
        [SerializeField] private UnityEvent<TMCardUIController, string> _onHoldCard = new();
        [SerializeField] private UnityEvent<TMCardUIController, int> _onDelayTurn = new();

        private TMCardData _cardData = null;
        private SmoothMoveVector2 _smoothMove = null;
        private Image _raycastingImage = null;
        private Image _cardImage = null;

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

        /// <summary>
        /// .. 카드가 덱어서 드로우 된 후 이벤트가 끝날때 호출되는 메서드 입니다
        /// </summary>
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
            if (!OnCard || CardData.IsAvailable(1)) return;

            UseCard();
        }
    }
}
