using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Components
{
    [DisallowMultipleComponent]
    public sealed class MovementTracker : MonoBehaviour
    {
        private const float THRESHOLD_LIMIT_MIN = 0.01f;
        private const float THRESHOLD_LIMIT_MAX = 0.5f;

        /// <summary>
        /// .. 움직임을 감지하는 오차범위
        /// </summary>
        public float PositionThreshold
        {
            get => _positionThreshold;
            set => _positionThreshold = Mathf.Clamp(value, THRESHOLD_LIMIT_MIN, THRESHOLD_LIMIT_MAX);
        }

        [SerializeField, Range(THRESHOLD_LIMIT_MIN, THRESHOLD_LIMIT_MAX)]
        private float _positionThreshold = 0.01f;

        private Vector3 _previousPosition = Vector3.zero;
        private bool _isMoving = false;
        private Action _onMoveBegined = null;
        private Action _onMoveOnGoing = null;
        private Action _onMoveEnded = null;

        private void Start()
        {
            _previousPosition = transform.position;
        }

        private void OnDisable()
        {
            _onMoveBegined = null;
            _onMoveOnGoing = null;
            _onMoveEnded = null;
        }

        private void FixedUpdate()
        {
            Vector3 currentPosition = transform.position;

            bool currentlyMoving = hasMoved(currentPosition, _previousPosition, _positionThreshold);

            if (currentlyMoving && !_isMoving)
            {
#if DEBUG
                Debug.Log("Move Begin");
#endif
                _onMoveBegined?.Invoke();
                _isMoving = true;
            }
            else if (!currentlyMoving && _isMoving)
            {
#if DEBUG
                Debug.Log("Move Ended");
#endif
                _onMoveEnded?.Invoke();
                _isMoving = false;
            }
            else if (currentlyMoving && _isMoving)
            {
#if DEBUG
                Debug.Log("Moving");
#endif
                _onMoveOnGoing?.Invoke();
            }

            _previousPosition = currentPosition;
        }

        public void AddListenerOnMoveBegined(Action action)
            => _onMoveBegined += action;

        public void AddListenerOnMoveOnGoing(Action action)
            => _onMoveOnGoing += action;

        public void AddListenerOnMoveEnded(Action action)
            => _onMoveEnded += action;

        public void RemoveListenerOnMoveBegined(Action action)
            => _onMoveBegined -= action;

        public void RemoveListenerOnMoveOnGoing(Action action)
            => _onMoveOnGoing -= action;

        public void RemoveListenerOnMoveEnded(Action action)
            => _onMoveEnded -= action;

        public void RemoveAllListenerOnMoveBegined()
            => _onMoveBegined = null;

        public void RemoveAllListenerOnMoveOnGoing()
            => _onMoveOnGoing = null;

        public void RemoveAllListenerOnMoveEnded()
            => _onMoveEnded = null;

        private bool hasMoved(Vector3 currentPosition, Vector3 previousPosition, float threshold)
            => Vector3.Distance(currentPosition, previousPosition) > threshold;
    }
}
