using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Components.Movement;
using Onw.ServiceLocator;
using TMCard.Effect;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace TMCard.Runtime
{
    // .. Model
    [DisallowMultipleComponent]
    public class TMCardModel : MonoBehaviour, ITMEffectTrigger
    {
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; }
        /// <summary>
        /// .. 카드가 현재 손 패 위에 있는지 확인하는 값입니다
        /// </summary>
        [field: Header("State")]
        [field: SerializeField, ReadOnly] public bool OnField { get; set; }

        [field: SerializeField, InitializeRequireComponent] public RectTransform RectTransform { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public TMCardInputHandler InputHandler { get; private set; }

        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover CardViewMover { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public Vector2SmoothMover CardBodyMover { get; private set; }

        public CardEvent OnEffectEvent { get; } = new();

        private bool _isInit;

        public void Initialize()
        {
            if (_isInit) return;

            _isInit = true;
            CardViewMover.IsLocal = true;
            CardBodyMover.IsLocal = true;

            InputHandler.AddListenerPointerEnterAction(eventData
                => CardViewMover.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

            InputHandler.AddListenerPointerExitAction(eventData
                => CardViewMover.TargetPosition = Vector2.zero);
            
            InputHandler.AddListenerPointerDownAction(onDownCard);
            
            void onDownCard(PointerEventData eventData)
            {
                if (!CardViewMover.enabled) return;

                Camera cardSystemCamera = eventData.enterEventCamera;
            
                setOnMover(CardViewMover, false);
                CardBodyMover.enabled = false;
            
                InputHandler.AddListenerPointerDragAction(onDrag);
                InputHandler.AddListenerPointerUpAction(onDragEnd);

                void onDrag(PointerEventData dragEventData)
                {
                    Vector2 mouseWorldPosition = cardSystemCamera.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
                    transform.localPosition = mouseLocalPosition;
                }

                void onDragEnd(PointerEventData dragEndEventData)
                {
                    InputHandler.RemoveListenerPointerDragAction(onDrag);
                    InputHandler.RemoveListenerPointerUpAction(onDragEnd);
                
                    setOnMover(CardViewMover, true);
                    CardBodyMover.enabled = true;
                }
                
                static void setOnMover(Vector2SmoothMover smoothMover, bool isOn)
                {
                    smoothMover.enabled = isOn;

                    if (!isOn)
                    {
                        smoothMover.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }

    }
}