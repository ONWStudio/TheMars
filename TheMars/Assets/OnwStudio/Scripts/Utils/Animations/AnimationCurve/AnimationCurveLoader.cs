using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AnimationCurveLoader : Singleton<AnimationCurveLoader>
{
    [SerializeField] private List<AnimationCurveReference> _animationCurveList = new();

    private readonly Dictionary<string, AnimationCurve> _animationCurves = new();
    public IReadOnlyDictionary<string, AnimationCurve> AnimationCurves => _animationCurves;

    protected override void Init()
    {
        _animationCurves.EnsureCapacity(_animationCurveList.Count);

        foreach (AnimationCurveReference animationCurveReference in _animationCurveList)
        {
            _animationCurves.Add(animationCurveReference.name, animationCurveReference.Curve);
        }

        _animationCurveList.Clear();
    }
}
