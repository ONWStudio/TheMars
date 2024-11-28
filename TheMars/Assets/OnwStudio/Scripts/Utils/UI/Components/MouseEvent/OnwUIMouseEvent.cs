using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Onw.Attribute;
using Onw.Components;
using UnityEngine.Events;

namespace Onw.UI.Components
{
    /// <summary>
    /// .. 미완성 버그 많음
    /// </summary>
    public sealed class OnwUIMouseEvent : MonoBehaviour
    {
        public event UnityAction<Vector2> OnPointerDown
        {
            add => _onPointerDown.AddListener(value);
            remove => _onPointerDown.RemoveListener(value);
        }
        
        public event UnityAction<Vector2> OnPointerUp
        {
            add => _onPointerUp.AddListener(value);
            remove => _onPointerUp.RemoveListener(value);
        }
        
        public event UnityAction<Vector2> OnPointerEnter
        {
            add => _onPointerEnter.AddListener(value);
            remove => _onPointerExit.RemoveListener(value);
        }
        
        public event UnityAction<Vector2> OnPointerExit
        {
            add => _onPointerExit.AddListener(value);
            remove => _onPointerExit.RemoveListener(value);
        }

        [field: SerializeField] public Camera TargetCamera { get; set; }
        [SerializeField, InitializeRequireComponent] private MouseInputEvent _mouseInputEvent;
        [SerializeField, InitializeRequireComponent] private RectTransform _rectTransform;

        [SerializeField] private UnityEvent<Vector2> _onPointerDown;
        [SerializeField] private UnityEvent<Vector2> _onPointerUp;
        [SerializeField] private UnityEvent<Vector2> _onPointerEnter;
        [SerializeField] private UnityEvent<Vector2> _onPointerExit;

        private bool _prevIsOver = false;
        
        private void Start()
        {
            _mouseInputEvent.AddListenerDownEvent<OnwMouseLeftInputEvent>(onMouseDownEvent);
            _mouseInputEvent.AddListenerUpEvent<OnwMouseLeftInputEvent>(onMouseUpEvent);
            MouseMovementTracker.Instance.OnHoverMouseForNowScene += onMouseHoverEvent;
        }

        private void onMouseHoverEvent(Vector2 mousePosition)
        {
            bool nowIsOver = isOverUI();

            if (_prevIsOver != nowIsOver)
            {
                if (_prevIsOver)
                {
                    _onPointerExit.Invoke(mousePosition);
                }
                else
                {
                    _onPointerEnter.Invoke(mousePosition);
                }
            }

            _prevIsOver = nowIsOver;
        }
        
        private void onMouseDownEvent(Vector2 mousePosition)
        {
            if (!isOverUI()) return;
            
            _onPointerDown.Invoke(mousePosition);
        }
        
        private void onMouseUpEvent(Vector2 mousePosition)
        {
            if (!isOverUI()) return;
            
            _onPointerUp.Invoke(mousePosition);
        }
        
        private bool isOverUI()
        {
            return RectTransformUtility.RectangleContainsScreenPoint(
                _rectTransform, 
                Mouse.current.position.ReadValue(), 
                TargetCamera);
        }
    }
}

