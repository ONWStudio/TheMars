using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Onw.Event;
using Onw.GridTile;
using Onw.Attribute;
using Onw.Extensions;
using Onw.ServiceLocator;
using Onw.Components.Movement;
using TM.Grid;
using TM.Building;
using TMCard.Effect;

namespace TMCard.Runtime
{
    // .. Model
    [DisallowMultipleComponent]
    public sealed class TMCardModel : MonoBehaviour, ITMEffectTrigger
    {
        [field: SerializeField, ReadOnly] public TMCardData CardData { get; set; }

        /// <summary>
        /// .. 카드가 현재 손 패 위에 있는지 확인하는 값입니다
        /// </summary>
        [field: Header("State")]
        [field: SerializeField, ReadOnly] public bool IsDragging { get; private set; } = false;
        [field: SerializeField, ReadOnly] public bool OnField { get; set; }

        [field: SerializeField, InitializeRequireComponent]
        public RectTransform RectTransform { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public TMCardInputHandler InputHandler { get; private set; }

        [field: Space]
        [field: Header("Require Option")]
        [field: SerializeField, SelectableSerializeField]
        public Vector2SmoothMover CardViewMover { get; private set; }

        [field: SerializeField, InitializeRequireComponent]
        public Vector2SmoothMover CardBodyMover { get; private set; }

        [field: SerializeField] public bool IsHide { get; set; } = false;
        public CardEvent OnEffectEvent { get; } = new();

        public IUnityEventListenerModifier OnDragBeginCard => _onDragBeginCard;
        public IUnityEventListenerModifier OnDragEndCard => _onDragEndCard;

        [SerializeField] private SafeUnityEvent _onDragBeginCard = new();
        [SerializeField] private SafeUnityEvent _onDragEndCard = new();

        [SerializeField, ReadOnly] private bool _isInit = false;

        public void Initialize()
        {
            if (_isInit) return;

            _isInit = true;
            CardViewMover.IsLocal = true;
            CardBodyMover.IsLocal = true;

            InputHandler.EnterAction.AddListener(eventData
                => CardViewMover.TargetPosition = 0.5f * RectTransform.rect.height * new Vector3(0f, 1f, 0f));

            InputHandler.ExitAction.AddListener(eventData
                => CardViewMover.TargetPosition = Vector2.zero);

            InputHandler.DownAction.AddListener(onDownCard);

            void onDownCard(PointerEventData eventData)
            {
                Camera cardSystemCamera = eventData.enterEventCamera;
                
                setOnMover(CardViewMover, false);
                CardBodyMover.enabled = false;

                InputHandler.DragAction.AddListener(onDrag);
                InputHandler.UpAction.AddListener(onDragEnd);
                IsDragging = true;

                _onDragBeginCard.Invoke();

                void onDrag(PointerEventData dragEventData)
                {
                    Vector2 mouseWorldPosition = cardSystemCamera.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mouseLocalPosition = transform.parent.InverseTransformPoint(mouseWorldPosition);
                    transform.localPosition = mouseLocalPosition;
                }

                void onDragEnd(PointerEventData dragEndEventData)
                {
                    InputHandler.DragAction.RemoveListener(onDrag);
                    InputHandler.UpAction.RemoveListener(onDragEnd);

                    setOnMover(CardViewMover, true);
                    CardBodyMover.enabled = true;
                    IsDragging = false;

                    _onDragEndCard.Invoke();
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