using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

/// <summary>
/// .. MM 피드백을 사용하는 이벤트 리시버 클래스 입니다. 파리미터로 들어온 이벤트 배열을 순차적으로 처리합니다
/// 이벤트들을 하나로 묶으려면 MMF_Paralle 이벤트 클래스를 참고해주세요
/// </summary>
[DisallowMultipleComponent]
public sealed class EventReceiver : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnComplitedEvent { get; private set; } = new();

    private MMF_Player _eventReceiver = null;

    private void Awake()
    {
        _eventReceiver = gameObject.AddComponent<MMF_Player>();
    }

    public void PlayEvent(params MMF_Feedback[] feedbacks)
    {
        _eventReceiver.StopFeedbacks();
        _eventReceiver.FeedbacksList.Clear();
        StartCoroutine(iEPlayFeedbacks(feedbacks));
    }

    private void onComplitedEvents()
    {
#if DEBUG
        Debug.Log("Event Complited!");
#endif
        OnComplitedEvent.Invoke();
    }

    private IEnumerator iEPlayFeedbacks(MMF_Feedback[] feedbacks)
    {
        foreach (MMF_Feedback feedback in feedbacks)
        {
            _eventReceiver.AddFeedback(feedback);
            _eventReceiver.Initialization();

            yield return _eventReceiver.PlayFeedbacksCoroutine(transform.position);
            _eventReceiver.FeedbacksList.Clear();
        }

        onComplitedEvents();
    }
}
