using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AnimcaitonCurve", menuName = "Scriptable Object/Animation Curve")]
    public sealed class AnimationCurveReference : ScriptableObject
    {
        [field: SerializeField] public AnimationCurve Curve { get; private set; } = null;
    }
}