using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.ScritableObjects
{
    [CreateAssetMenu(fileName = "AnimcaitonCurve", menuName = "Scriptable Object/Animation Curve")]
    public sealed class AnimationCurveReference : ScriptableObject
    {
        [field: SerializeField] public AnimationCurve Curve { get; private set; } = null;

        private void OnValidate()
        {
            for (int i = 0; i < Curve.keys.Length; i++)
            {
                Debug.Log("in : " + Curve.keys[i].inTangent);
                Debug.Log("out : " + Curve.keys[i].outTangent);
            }
        }
    }
}