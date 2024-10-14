using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Onw.Components
{
    public sealed class MouseMovementTracker : MonoBehaviour
    {
        private const float MOVEMENT_THRESHOLD_MIN = 0f;
        private const float MOVEMENT_THRESHOLD_MAX = 0.25f;

        [field: SerializeField, Range(MOVEMENT_THRESHOLD_MIN, MOVEMENT_THRESHOLD_MAX)] public float MovementThreshold = 0.05f;

        public event UnityAction<Vector2> OnMoveBeginMouse
        {
            add => _onMoveBeginMouse.AddListener(value);
            remove => _onMoveBeginMouse.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnMoveEndMouse
        {
            add => _onMoveEndMouse.AddListener(value);
            remove => _onMoveEndMouse.RemoveListener(value);
        }

        public event UnityAction<Vector2> OnHoverMouse
        {
            add => _onHoverMouse.AddListener(value);
            remove => _onHoverMouse.RemoveListener(value);
        }

        private Vector3 _prevCursorPoint = Vector3.zero;
        private bool _isMove = false;

        [FormerlySerializedAs("_onMoveMouse")]
        [field: SerializeField] private UnityEvent<Vector2> _onMoveBeginMouse = new();
        [field: SerializeField] private UnityEvent<Vector2> _onMoveEndMouse = new();
        [field: SerializeField] private UnityEvent<Vector2> _onHoverMouse = new();

        private void Start()
        {
            _prevCursorPoint = Input.mousePosition;
        }

        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 delta = new(
                Mathf.Abs(_prevCursorPoint.x - mousePosition.x),
                Mathf.Abs(_prevCursorPoint.y - mousePosition.y));

            bool isHover = delta.sqrMagnitude > MovementThreshold * MovementThreshold;

            if (isHover)
            {
                _onHoverMouse.Invoke(mousePosition);
                _prevCursorPoint = mousePosition;
                
                if (!_isMove)
                {
                    _isMove = true;
                    _onMoveBeginMouse.Invoke(mousePosition);
                }
            }
            else
            {
                if (_isMove)
                {
                    _isMove = false;
                    _onMoveEndMouse.Invoke(mousePosition);
                }
            }
        }
    }
}