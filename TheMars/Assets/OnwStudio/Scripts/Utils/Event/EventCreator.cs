using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

/// <summary>
/// .. 단순 편의성을 위한 크리에이터 클래스 입니다 사용하지 않아도 됩니다.
/// </summary>
public static class EventCreator
{
    public static MMTweenType SmootyTween { get; private set; } = new(MMTween.MMTweenCurve.EaseOutQuintic);

    public static MMF_Rotation CreateSmoothRotationEvent(Transform animateRotationTarget, Vector3 targetEulerAngle, float feedbackDuration = 1.0f) => new()
    {
        Label = "Rotation",
        AnimateRotationTarget = animateRotationTarget,
        DestinationAngles = targetEulerAngle,
        Mode = MMF_Rotation.Modes.ToDestination,
        RotationSpace = Space.Self,
        FeedbackDuration = feedbackDuration,
        ToDestinationTween = SmootyTween
    };

    public static MMF_Position CreateSmoothPositionEvent(GameObject animatePositionTarget, Vector3 targetPosition, float feedbackDuration = 1.0f) => new()
    {
        Label = "Position",
        AnimatePositionTarget = animatePositionTarget,
        DestinationPosition = targetPosition,
        Mode = MMF_Position.Modes.ToDestination,
        Space = MMF_Position.Spaces.Local,
        FeedbackDuration = 1.0f,
        RelativePosition = false,
        AnimatePositionTween = SmootyTween
    };
}
