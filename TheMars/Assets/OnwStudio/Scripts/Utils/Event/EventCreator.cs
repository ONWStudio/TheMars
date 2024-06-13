using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public static MMF_Parallel CreateSmoothPositionAndRotationEvent(GameObject animateTarget, Vector3 targetPosition, Vector3 targetEulerAngle, float feedbackDuration = 1.0f)
    {
        MMF_Rotation mmfRotation = CreateSmoothRotationEvent(animateTarget.transform, targetEulerAngle, feedbackDuration);
        MMF_Position mmfPosition = CreateSmoothPositionEvent(animateTarget, targetPosition, feedbackDuration);

        MMF_Parallel mmfParallel = new();
        mmfParallel.Feedbacks.Add(mmfRotation);
        mmfParallel.Feedbacks.Add(mmfPosition);

        return mmfParallel;
    }

    /// <summary>
    /// .. null 초기화시 이벤트가 등록되지 않습니다
    /// </summary>
    /// <param name="playEvent"></param>
    /// <param name="stopEvent"></param>
    /// <param name="initializeEvent"></param>
    /// <param name="resetEvent"></param>
    /// <returns></returns>
    public static MMF_Events CreateUnityEvent(UnityAction playEvent, UnityAction stopEvent, UnityAction initializeEvent, UnityAction resetEvent)
    {
        MMF_Events mmfEvents = new()
        {
            PlayEvents = createNewEvent(playEvent),
            StopEvents = createNewEvent(stopEvent),
            InitializationEvents = createNewEvent(initializeEvent),
            ResetEvents = createNewEvent(resetEvent)
        };

        return mmfEvents;
    }

    private static UnityEvent createNewEvent(UnityAction unityAction)
    {
        if (unityAction == null) return null;

        UnityEvent unityEvent = new();
        unityEvent.AddListener(unityAction);

        return unityEvent;
    }
}
