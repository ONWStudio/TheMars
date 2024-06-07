using System;
using System.Collections;
using System.Collections.Generic;
using CoroutineExtensions;
using UnityEngine;

// .. 사용하지 않음 레거시
[DisallowMultipleComponent, RequireComponent(typeof(MovementTracker))]
public abstract class CardMovementBase : MonoBehaviour
{
    [Serializable]
    public struct TransformData
    {
        [field: SerializeField] public Quaternion Rotation { get; set; }
        [field: SerializeField] public Vector3 Position { get; set; }
        [field: SerializeField] public Vector3 Offset { get; set; }
    }

    [field: SerializeField] public TransformData CurrentTransform { get; protected set; }

    public TransformData TargetTransform
    {
        get => _targetTransform;
        set
        {
            CurrentTransform = _targetTransform;
            _targetTransform = value;
            _normalizedValue = 0f;
        }
    }

    public float NormalizedValue
    {
        get => _normalizedValue;
        protected set
        {
            _normalizedValue = Mathf.Clamp01(value);

            transform.SetLocalPositionAndRotation(
                Vector3.Lerp(
                    CurrentTransform.Position + CurrentTransform.Offset,
                    TargetTransform.Position + TargetTransform.Offset,
                    _normalizedValue),
                Quaternion.Slerp(
                    CurrentTransform.Rotation,
                    TargetTransform.Rotation,
                    _normalizedValue));
        }
    }

    [SerializeField] TransformData _targetTransform;
    [SerializeField, Range(0f, 1f)] private float _normalizedValue = 0f;

    private MovementTracker _movementTracker = null;

    protected abstract void SetNormalized();

    private void Awake()
    {
        _movementTracker = GetComponent<MovementTracker>();
    }

    private void Start()
    {
        CurrentTransform = new()
        {
            Rotation = transform.rotation,
            Position = transform.position
        };

        _movementTracker.AddListenerOnMoveBegined(()
            => (this as ICardMoveBegin)?.OnMoveBegin());

        _movementTracker.AddListenerOnMoveOnGoing(()
            => (this as ICardMove)?.OnMove());

        _movementTracker.AddListenerOnMoveEnded(()
            => (this as ICardMoveEnd)?.OnMoveEnd());
    }

    private void FixedUpdate()
    {
        SetNormalized();
    }

    private void OnValidate()
    {
        NormalizedValue = _normalizedValue;
    }
}
