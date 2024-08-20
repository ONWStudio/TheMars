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

        [field: Header("Event")]
        [field: SerializeField] public SafeUnityEvent<Transform> OnChangedParent { get; private set; } = new();
        [field: SerializeField, InitializeRequireComponent] public RectTransform RectTransform { get; private set; }

        [field: FormerlySerializedAs("_inputHandler")]
        [field: FormerlySerializedAs("inputHandler")]
        [field: Header("Input Handler")]
        [field: SerializeField, InitializeRequireComponent]
        public TMCardInputHandler InputHandler { get; private set; }
        
        [field: FormerlySerializedAs("_smoothMove")]
        [field: FormerlySerializedAs("smoothMove")]
        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover SmoothMove { get; private set; }
        
        public IReadOnlyList<ITMCardEffect> Effects => _cardEffects;

        public CardEvent OnClickEvent { get; } = new();
        public CardEvent OnDrawBeginEvent { get; } = new();
        public CardEvent OnDrawEndedEvent { get; } = new();
        public CardEvent OnTurnEndedEvent { get; } = new();
        public CardEvent OnEffectEvent { get; } = new();

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

            initializeInputHandle();
            initializeSmoothMove();

            OnClickEvent.AddListener(eventState =>
            {
                this.MoveToScreenCenterAfterToTomb();
                OnEffectEvent.Invoke(eventState);
            });

            OnTurnEndedEvent.AddListener(eventState => this.MoveToTomb());
            CardData.ApplyEffect(this);
        }

        public void SetEffect(IEnumerable<ITMCardEffect> effects)
        {
            _cardEffects.AddRange(effects);
            _cardEffects.ForEach(effect => effect?.ApplyEffect(this, this));
        }

        /// <summary>
        /// .. 카드가 덱에서 드로우 될 때 호출되는 메서드 입니다
        /// </summary>
        public void OnDrawBegin()
        {
            OnDrawBeginEvent.Invoke(CardEventState.DRAW);
        }

        public void OnDrawEnded()
        {
            OnDrawEndedEvent.Invoke(CardEventState.DRAW);
        }

        /// <summary>
        /// .. 턴이 종료되었을때 TurnEnd메서드를 호출합니다
        /// </summary>
        public void OnTurnEnd()
        {
            OnTurnEndedEvent.Invoke(CardEventState.TURN_END);
        }

        /// <summary>
        /// .. 카드의 상호작용 상태를 전환 하는 메서드 입니다 false 일시 카드와 상호작용이 불가능 합니다
        /// </summary>
        /// <param name="isOn"> .. 카드의 상호작용 상태를 전환시킬 boolen 값 </param>
        public void SetOn(bool isOn)
        {
            SmoothMove.enabled = isOn;

            if (!isOn)
            {
                SmoothMove.transform.localPosition = Vector3.zero;
            }
        }

        private void initializeSmoothMove()
        {
            SmoothMove.IsLocal = true;
        }

        private void initializeInputHandle()
        {
            InputHandler.AddListenerPointerEnterAction(pointerEventData =>
                SmoothMove.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

            InputHandler.AddListenerPointerExitAction(pointerEventData
                => SmoothMove.TargetPosition = Vector2.zero);

            InputHandler.AddListenerPointerClickAction(OnClickCard);
        }

        /// <summary>
        /// .. 카드를 사용합니다
        /// 카드 사용시 카드는 효과가 발동되며 카드 효과는 이벤트 기반입니다 
        /// </summary>
        /// <param name="pointerEventData"></param>
        private void OnClickCard(PointerEventData pointerEventData)
        {
            if (!SmoothMove.enabled || !ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            // if (!CardData.IsAvailable(1)) return;

            OnClickEvent.Invoke(CardEventState.NORMAL);
            service.OnClickCard(this);
        }
    }
}
