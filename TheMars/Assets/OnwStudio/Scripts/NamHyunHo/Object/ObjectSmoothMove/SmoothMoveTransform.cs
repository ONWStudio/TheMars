using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class SmoothMoveTransform : MonoBehaviour, IObjectSmoothMove
{
    [field: Header("Target")]
    [field: SerializeField] public Transform TargetTransform { get; set; } = null;

    [field: Header("Move Option")]
    [field: SerializeField, Range(0.15f, 1f)] public float Ratio { get; set; } = 0.45f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!TargetTransform) return;

        transform.position += (this as IObjectSmoothMove).GetVector(transform.position, TargetTransform.position);
    }
}
