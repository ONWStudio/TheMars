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
    /// <summary>
    /// .. TMCardController는 동작에 대한 정의만 수행합니다 카드의 구체적인 동작 사이클을 컨트롤러와 
    /// CardStateMachine이 수행하며 필요한 동작에 대한 구체적인 구현은 각 콜백 메서드에 바인딩하여 외부에서 구현합니다
    /// 컨트롤러는 카드 데이터, EventSender, InputHandler와 각 동작에 대한 이벤트에 대한 인터페이스만을 제공하며 다른 기능은 포함하고 있지 않습니다
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardUIController : MonoBehaviour, ITMCardController<TMCardUIController>
    {
        public UnityEvent<TMCardUIController> OnUseCard => _onUseCard;
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
        /// .. 카드가 상호작용 가능한 활성화인지 상태를 반환합니다
        /// </summary>
        public bool OnCard { get; private set; } = false;
        /// <summary>
        /// .. 카드의 상세한 기본 데이터 입니다
        /// </summary>
        public TMCardData CardData => _cardData;

        [SerializeField] private UnityEvent<TMCardUIController> _onUseCard = new();
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

        public void OnUseStart()
        {
            _cardData.StateMachine.OnUseStarted(this);
        }

        public void OnUseEnded()
        {
            _cardData.StateMachine.OnUseEnded(this);
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

        public void InitializeCard(TMCardData cardData)
        {
            if (_cardData) return;

            _cardData = cardData;
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

            OnUseCard.Invoke(this);
        }
    }
}
