using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.Components.Movement;
using Onw.Extensions;
using Onw.Event;
using UnityEngine.EventSystems;

namespace TMCard.UI
{
    // .. Model
    [DisallowMultipleComponent]
    public sealed class TMCardController : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; } = null;
        /// <summary>
        /// .. 이벤트 센더 클래스 입니다 외부에서 이벤트 연출 효과를 발생시킬때 사용할 수 있는 프로퍼티 입니다
        /// </summary>
        [field: Header("Event Sender")]
        [field: SerializeField, InitializeRequireComponent] public EventSender EventSender { get; private set; } = null;
        /// <summary>
        /// .. 카드에 사용자 입력을 받아오는 핸들러 입니다
        /// </summary>
        [field: Header("Input Handler")]
        [field: SerializeField, InitializeRequireComponent] public TMCardInputHandler InputHandler { get; private set; } = null;

        [field: Header("State")]
        [field: SerializeField, ReadOnly] public bool OnField { get; set; } = false;
        /// <summary>
        /// .. 카드가 상호작용 가능한 활성화인지 상태를 반환합니다
        /// </summary>
        [field: SerializeField, ReadOnly] public bool OnCard { get; private set; } = false;

        [field: Header("Event")]
        [field: SerializeField] public UnityEvent<Transform> OnChangedParent { get; private set; } = new();
        [field: SerializeField, InitializeRequireComponent] public RectTransform RectTransform { get; private set; } = null;

        public Action UseState { get; set; } = null;
        public Action DrawBeginState { get; set; } = null;
        public Action DrawEndedState { get; set; } = null;
        public Action TurnEndedState { get; set; } = null;

        [Space]
        /// <summary>
        /// .. 카드의 상세한 기본 데이터 입니다
        /// </summary>
        [Header("Require Option")]
        [SerializeField] private Vector2SmoothMover _smoothMove = null;

        public void Initialize()
        {
            initalizeInputHandle();
            initializeSmoothMove();

            UseState = () =>
            {
                CardData.UseCard();
                TMCardGameManager.Instance.EffectCard(this);
            };

            TurnEndedState = () => TMCardGameManager.Instance.MoveToTomb(this);
            CardData.ApplySpecialEffect(this);
        }

        public void OnUsed()
        {
            UseState?.Invoke();
        }

        private void OnTransformParentChanged()
        {
            OnChangedParent.Invoke(transform.parent);
        }

        /// <summary>
        /// .. 카드가 덱에서 드로우 될 때 호출되는 메서드 입니다
        /// </summary>
        public void OnDrawBegin()
        {
            DrawBeginState?.Invoke();
        }

        public void OnDrawEnded()
        {
            DrawEndedState?.Invoke();
        }

        /// <summary>
        /// .. 턴이 종료되었을때 TurnEnd메서드를 호출합니다
        /// </summary>
        public void OnTurnEnd()
        {
            TurnEndedState?.Invoke();
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
            InputHandler.AddListenerPointerEnterAction(pointerEventData =>
                _smoothMove.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

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
            if (!OnCard || !CardData.IsAvailable(1)) return;

            TMCardGameManager.Instance.OnClickCard(this);
        }
    }
}
