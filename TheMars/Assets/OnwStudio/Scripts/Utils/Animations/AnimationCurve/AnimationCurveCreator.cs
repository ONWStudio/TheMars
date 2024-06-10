using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class AnimationCurveCreator
{ 
    public static AnimationCurve GetSmoothCurve()
    {
        AnimationCurve smoothCurve = new();
        smoothCurve.AddKey(new Keyframe(0.0f, 0.0f, 2.0f, 2.0f));
        smoothCurve.AddKey(new Keyframe(1.0f, 1.0f, 0.0f, 0.0f));

        smoothCurve.postWrapMode = WrapMode.Clamp;
        smoothCurve.preWrapMode = WrapMode.Clamp;

        return smoothCurve;
    }
}
