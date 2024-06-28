using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Onw.Components.Movement
{
    [DisallowMultipleComponent]
    public sealed class Vector2SmoothMover : MonoBehaviour, IObjectSmoothMover
    {
        [field: Header("Target")]
        [field: SerializeField] public Vector2 TargetPosition { get; set; } = Vector2.positiveInfinity;

        [field: Header("Move Options")]
        [field: SerializeField] public float Ratio { get; set; } = 0.45f;
        [field: SerializeField] public bool IsLocal { get; set; } = false;

        private void FixedUpdate()
        {
            if (float.IsInfinity(TargetPosition.x) || float.IsInfinity(TargetPosition.y)) return;

            Vector3 target = (this as IObjectSmoothMover)
                .GetVector(
                IsLocal ? transform.localPosition : transform.position,
                (Vector3)TargetPosition);

            if (IsLocal)
            {
                transform.localPosition += new Vector3(target.x, target.y, 0f);
            }
            else
            {
                transform.position += new Vector3(target.x, target.y, 0f);
            }
        }
    }
}
