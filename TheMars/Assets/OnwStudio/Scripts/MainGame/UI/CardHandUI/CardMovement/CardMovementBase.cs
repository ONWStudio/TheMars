using System;
using System.Collections;
using System.Collections.Generic;
using CoroutineExtensions;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class CardMovementBase : MonoBehaviour
{
    [Serializable]
    public struct TransformData
    {
        [field: SerializeField] public Quaternion Rotation { get; set; }
        [field: SerializeField] public Vector3 Position { get; set; }

        [field: SerializeField] public Vector3 Offset { get; set; }
    }

    [field: SerializeField] public TransformData TargetTransform { get; set; }
    [field: SerializeField] public TransformData CurrentTransform { get; protected set; }

    public float NormalizedValue
    {
        get => _normalizedValue;
        protected set
        {
            _normalizedValue = Mathf.Clamp01(value);

            transform.SetLocalPositionAndRotation(
                Vector3.Lerp(CurrentTransform.Position + CurrentTransform.Offset, TargetTransform.Position + TargetTransform.Offset, _normalizedValue) ,
                Quaternion.Slerp(CurrentTransform.Rotation, TargetTransform.Rotation, _normalizedValue));
        }
    }

    [SerializeField, Range(0f, 1f)] private float _normalizedValue = 0f;

    public abstract void MoveCard();
    protected abstract void SetNormalized();

    protected IEnumerator iEOnMove()
    {
        (this as ICardMoveBegin)?.OnMoveBegin();
        ICardMove cardMoveHandler = this as ICardMove;

        NormalizedValue = 0f;
        CurrentTransform = new()
        {
            Position = transform.localPosition,
            Rotation = transform.localRotation
        };

        while (NormalizedValue < 0.98f)
        {
            SetNormalized();
            cardMoveHandler?.OnMove();
            yield return CoroutineHelper.WaitForFixedUpdate;
        }

        NormalizedValue = 1f;
        CurrentTransform = TargetTransform;

        (this as ICardMoveEnd)?.OnMoveEnd();
    }

    private void Start()
    {
        CurrentTransform = new()
        {
            Rotation = transform.rotation,
            Position = transform.position
        };
    }

    private void OnValidate()
    {
        NormalizedValue = _normalizedValue;
    }
}
