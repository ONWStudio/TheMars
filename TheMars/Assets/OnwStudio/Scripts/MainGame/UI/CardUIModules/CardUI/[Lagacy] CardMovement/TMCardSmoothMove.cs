using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineExtensions;

namespace TMCardUISystemModules
{
    /// <summary>
    /// .. 사용하지 않음
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardSmoothMove : TMCardMovementBase
    {
        [field: SerializeField, Range(0.25f, 1f)] public float Ratio { get; set; } = 0.25f;

        protected override void SetNormalized()
            => NormalizedValue += (1f - NormalizedValue) * Ratio;
    }
}
