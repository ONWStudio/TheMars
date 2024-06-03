using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineExtensions;

public sealed class SmoothMove : CardMovementBase
{
    [field: SerializeField, Range(0.25f, 1f)] public float Ratio { get; set; } = 0.25f;

    public override void MoveCard()
    {
        StopCoroutine(iEOnMove());
        StartCoroutine(iEOnMove());
    }

    protected override void SetNormalized()
    {
        NormalizedValue += (1f - NormalizedValue) * Ratio;
    }
}
