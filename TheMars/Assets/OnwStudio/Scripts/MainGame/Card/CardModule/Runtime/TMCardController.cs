using System.Collections.Generic;
using Onw.Attribute;
using Onw.Components.Movement;
using Onw.ServiceLocator;
using TMCard.Effect;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Onw.Event;

namespace TMCard.Runtime
{
    // .. Model
    [DisallowMultipleComponent]
    public class TMCardController : MonoBehaviour, ITMEffectTrigger
    {
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; }
        /// <summary>
        /// .. 카드가 현재 손 패 위에 있는지 확인하는 값입니다
        /// </summary>
        [field: Header("State")]
        [field: SerializeField, ReadOnly] public bool OnField { get; set; }
        /// <summary>
        /// .. 카드가 상호작용 가능한 활성화인지 상태를 반환합니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool OnCard { get; private set; }

        [field: Header("Event")]
        [field: SerializeField] public SafeUnityEvent<Transform> OnChangedParent { get; private set; } = new();
        [field: SerializeField, InitializeRequireComponent] public RectTransform RectTransform { get; private set; }

        public IReadOnlyList<ITMCardEffect> Effects => _cardEffects;

        public CardEvent OnClickEvent { get; } = new();
        public CardEvent OnDrawBeginEvent { get; } = new();
        public CardEvent OnDrawEndedEvent { get; } = new();
        public CardEvent OnTurnEndedEvent { get; } = new();
        public CardEvent OnEffectEvent { get; } = new();

        [FormerlySerializedAs("inputHandler")]
        [Header("Input Handler")]
        [SerializeField, InitializeRequireComponent]
        private TMCardInputHandler _inputHandler;

        [FormerlySerializedAs("smoothMove")]
        [Space]
        [Header("Require Option")]
        [SerializeField, SelectableSerializeField]
        private Vector2SmoothMover _smoothMove;

        private readonly List<ITMCardEffect> _cardEffects = new();
        private bool _isInit;

        private void OnTransformParentChanged()
        {
            OnChangedParent.Invoke(transform.parent);
        }

        public void Initialize()
        {
            if (_isInit) return;

            _isInit = true;

            initalizeInputHandle();
            initializeSmoothMove();

            OnClickEvent.AddListener(() =>
            {
                this.MoveToScreenCenterAfterToTomb();
                OnEffectEvent.Invoke();
            });

            OnTurnEndedEvent.AddListener(this.MoveToTomb);
            CardData.ApplyEffect(this);
        }

        public void SetEffect(IEnumerable<ITMCardEffect> effects)
        {
            _cardEffects.AddRange(effects);
            _cardEffects.ForEach(effect => effect.ApplyEffect(this, this));
        }

        /// <summary>
        /// .. 카드가 덱에서 드로우 될 때 호출되는 메서드 입니다
        /// </summary>
        public void OnDrawBegin()
        {
            OnDrawBeginEvent.Invoke();
        }

        public void OnDrawEnded()
        {
            OnDrawEndedEvent.Invoke();
        }

        /// <summary>
        /// .. 턴이 종료되었을때 TurnEnd메서드를 호출합니다
        /// </summary>
        public void OnTurnEnd()
        {
            OnTurnEndedEvent.Invoke();
        }

        /// <summary>
        /// .. 카드의 상호작용 상태를 전환 하는 메서드 입니다 false 일시 카드와 상호작용이 불가능 합니다
        /// </summary>
        /// <param name="isOn"> .. 카드의 상호작용 상태를 전환시킬 boolen 값 </param>
        public void SetOn(bool isOn)
        {
            OnCard = isOn;
            _smoothMove.enabled = isOn;

            if (!isOn)
            {
                _smoothMove.transform.localPosition = Vector3.zero;
            }
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
            _inputHandler.AddListenerPointerEnterAction(pointerEventData =>
                _smoothMove.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

            _inputHandler.AddListenerPointerExitAction(pointerEventData
                => _smoothMove.TargetPosition = Vector2.zero);

            _inputHandler.AddListenerPointerClickAction(OnClickCard);
        }

        /// <summary>
        /// .. 카드를 사용합니다
        /// 카드 사용시 카드는 효과가 발동되며 카드 효과는 이벤트 기반입니다 
        /// </summary>
        /// <param name="pointerEventData"></param>
        private void OnClickCard(PointerEventData pointerEventData)
        {
            if (!OnCard || !ServiceLocator<ITMCardService>.TryGetService(out var service)) return;
            // if (!CardData.IsAvailable(1)) return;

            OnClickEvent.Invoke();
            service.OnClickCard(this);
        }
    }
}
