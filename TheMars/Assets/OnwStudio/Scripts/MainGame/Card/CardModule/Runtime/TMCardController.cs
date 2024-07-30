using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Onw.Components.Movement;
using Onw.Extensions;
using Onw.Interface;
using Onw.Attribute;
using Onw.Event;
using TMCard.Effect;
using TMCard.Effect.Resource;

namespace TMCard.Runtime
{
    public readonly struct TMCardEffectArgs
    {
        public bool HasDescription { get; }
        public bool HasLabel { get; }

        public string Description { get; }
        public string Label { get; }

        public TMCardEffectArgs(bool hasDescription, bool hasLabel, string description, string label)
        {
            HasDescription = hasDescription;
            HasLabel = hasLabel;
            Description = description;
            Label = label;
        }
    }

    // .. Model
    [DisallowMultipleComponent]
    public class TMCardController : MonoBehaviour, ITMEffectTrigger
    {
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; } = null;
        /// <summary>
        /// .. 이벤트 센더 클래스 입니다 외부에서 이벤트 연출 효과를 발생시킬때 사용할 수 있는 프로퍼티 입니다
        /// </summary>
        [field: Header("Event Sender")]
        [field: SerializeField, InitializeRequireComponent] public EventSender EventSender { get; private set; } = null;
        /// <summary>
        /// .. 카드가 현재 손 패 위에 있는지 확인하는 값입니다
        /// </summary>
        [field: Header("State")]
        [field: SerializeField, ReadOnly] public bool OnField { get; set; } = false;
        /// <summary>
        /// .. 카드가 상호작용 가능한 활성화인지 상태를 반환합니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool OnCard { get; private set; } = false;

        [field: Header("Event")]
        [field: SerializeField] public UnityEvent<Transform> OnChangedParent { get; private set; } = new();
        [field: SerializeField, InitializeRequireComponent] public RectTransform RectTransform { get; private set; } = null;

        public IEnumerable<TMCardEffectArgs> EffectArgs
        {
            get
            {
                foreach (ITMCardEffect effect in _cardEffects)
                {
                    bool hasDescription = false;
                    bool hasLabel = false;
                    string description = string.Empty;
                    string labelStr = string.Empty;

                    if (effect is IDescriptable descriptable)
                    {
                        hasDescription = true;
                        description = descriptable.Description;
                    }

                    if (effect is ILabel label)
                    {
                        hasLabel = true;
                        labelStr = label.Label;
                    }

                    yield return new(hasDescription, hasLabel, description, labelStr);
                }
            }
        }

        public IEnumerable<ITMCardResourceEffect> ResourceEffects
        {
            get
            {
                foreach (ITMCardEffect effect in _cardEffects)
                {
                    if (effect is not ITMCardResourceEffect resourceEffect) continue;

                    yield return resourceEffect;
                }
            }
        }

        public CardEvent OnClickEvent { get; } = new();
        public CardEvent OnDrawBeginEvent { get; } = new();
        public CardEvent OnDrawEndedEvent { get; } = new();
        public CardEvent OnTurnEndedEvent { get; } = new();
        public CardEvent OnEffectEvent { get; } = new();

        [Header("Input Handler")]
        [SerializeField, InitializeRequireComponent] private TMCardInputHandler _inputHandler = null;

        [Space]
        /// <summary>
        /// .. 카드의 상세한 기본 데이터 입니다
        /// </summary>
        [Header("Require Option")]
        [SerializeField, SelectableSerializeField] private Vector2SmoothMover _smoothMove = null;

        private readonly List<ITMCardEffect> _cardEffects = new();
        private bool _isInitEffect = false;

        private void OnTransformParentChanged()
        {
            OnChangedParent.Invoke(transform.parent);
        }

        public void Initialize()
        {
            initalizeInputHandle();
            initializeSmoothMove();

            OnClickEvent.AddListener(() =>
            {
                TMCardGameManager.Instance.MoveToScreenCenterAfterToTomb(this);
                OnEffectEvent.Invoke();
            });

            OnTurnEndedEvent.AddListener(() => TMCardGameManager.Instance.MoveToTomb(this));
            CardData.ApplyEffect(this);
        }

        public void SetEffect(IEnumerable<ITMCardEffect> effects)
        {
            if (_isInitEffect) return;

            _isInitEffect = true;
            _cardEffects.AddRange(effects);
            _cardEffects.ForEach(effect => effect.ApplyEffect(this, this));
        }

        public void OnUsed()
        {
            OnClickEvent.Invoke();
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

            _inputHandler.AddListenerPointerClickAction(onClickCard);
        }

        /// <summary>
        /// .. 카드를 사용합니다
        /// 카드 사용시 카드는 효과가 발동되며 카드 효과는 이벤트 기반입니다 
        /// </summary>
        /// <param name="pointerEventData"></param>
        private void onClickCard(PointerEventData pointerEventData)
        {
            if (!OnCard || !CardData.IsAvailable(1)) return;

            TMCardGameManager.Instance.OnClickCard(this);
        }
    }
}
