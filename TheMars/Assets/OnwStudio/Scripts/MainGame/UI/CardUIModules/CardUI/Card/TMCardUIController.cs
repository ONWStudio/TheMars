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
    public sealed class TMCardUIController : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent<TMCardUIController> OnUseStarted { get; private set; } = new();
        [field: SerializeField] public UnityEvent<TMCardUIController> OnUseEnded { get; private set; } = new();

        [field: SerializeField] public UnityEvent<TMCardUIController> OnMoveToTomb { get; private set; } = new();
        [field: SerializeField] public UnityEvent<TMCardUIController> OnDrawFromDeck { get; private set; } = new();

        public EventReceiver EventReceiver { get; private set; } = null;
        public TMCardInputHandler InputHandler { get; private set; } = null;
        public TMCardData CardData { get; private set; } = null;

        private SmoothMoveVector2 _smoothMove = null;
        private RectTransform _rectTransform = null;
        private Image _raycastingImage = null;
        private Image _cardImage = null;

        public bool OnCard { get; private set; } = false;

        private void Awake()
        {
            EventReceiver = gameObject.AddComponent<EventReceiver>();
            InputHandler = gameObject.AddComponent<TMCardInputHandler>();
            _raycastingImage = gameObject.AddComponent<Image>();
            _cardImage = new GameObject("CardImage").AddComponent<Image>();
            _smoothMove = _cardImage.gameObject.AddComponent<SmoothMoveVector2>();
            _rectTransform = transform as RectTransform;
        }

        private void Start()
        {
            _raycastingImage.color = new(255f, 255f, 255f, 0f);

            _cardImage.transform.SetParent(transform, false);
            _cardImage.raycastTarget = false;
            _cardImage.transform.localPosition = Vector3.zero;

            _smoothMove.IsLocal = true;

            InputHandler.AddListenerPointerEnterAction(pointerEventData
                => _smoothMove.TargetPosition = 0.5f * _rectTransform.rect.height * new Vector3(0f, 1f, 0f));

            InputHandler.AddListenerPointerExitAction(pointerEventData
                => _smoothMove.TargetPosition = Vector2.zero);

            InputHandler.AddListenerPointerClickAction(onClickCard);

            if (!OnCard)
            {
                _smoothMove.enabled = false;
            }
        }

        /// <summary>
        /// .. 카드를 사용합니다
        /// 카드 사용시 카드는 효과가 발동되며 카드 효과는 이벤트 기반입니다 
        /// </summary>
        /// <param name="pointerEventData"></param>
        private void onClickCard(PointerEventData pointerEventData)
        {
            if (!OnCard) return;

            _smoothMove.enabled = false;
            _smoothMove.transform.localPosition = Vector3.zero;

            SetOn(false);

            OnUseStarted.Invoke(this);
            OnUseStarted.RemoveAllListeners();

            List<MMF_Feedback> events = new(); //new(CardData
            //    .CardEffects
            //    .SelectMany(cardEffect => cardEffect.OnEffectStart(gameObject, CardData)));

            Vector3 targetWorldPosition = pointerEventData
                .enterEventCamera
                .ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.5f));

            Vector3 targetPosition = transform
                .parent
                .InverseTransformPoint(new(targetWorldPosition.x, targetWorldPosition.y, 0f));

            MMF_Parallel parallelEvent = new();

            parallelEvent.Feedbacks.Add(EventCreator
                .CreateSmoothPositionEvent(gameObject, new(targetPosition.x, targetPosition.y, 0f)));

            parallelEvent.Feedbacks.Add(EventCreator
                .CreateSmoothRotationEvent(transform, Vector3.zero));

            MMF_Events endCallbackEvent = new()
            {
                PlayEvents = new()
            };

            events.Add(parallelEvent);
            events.Add(endCallbackEvent);
            //events.AddRange(CardData
            //    .CardEffects
            //    .SelectMany(cardEffect => cardEffect.OnEffectEnded(gameObject, CardData)));

            endCallbackEvent.PlayEvents.AddListener(() =>
            {
                OnUseEnded.Invoke(this);
                OnUseEnded.RemoveAllListeners();
            });

            EventReceiver.PlayEvent(events.ToArray());
        }

        public void SetOn(bool isOn)
        {
            OnCard = isOn;
            _smoothMove.enabled = isOn;
        }
    }
}