using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public sealed class SmoothMoveVector2 : MonoBehaviour, IObjectSmoothMove
{
    [field: Header("Target")]
    [field: SerializeField] public Vector2 TargetPosition { get; set; } = Vector2.positiveInfinity;

    [field: Header("Move Options")]
    [field: SerializeField] public float Ratio { get; set; } = 0.45f;

    private void FixedUpdate()
    {
        if (float.IsInfinity(TargetPosition.x) || float.IsInfinity(TargetPosition.y)) return;

        transform.position += (this as IObjectSmoothMove).GetVector(transform.position, (Vector3)TargetPosition);
    }
}
