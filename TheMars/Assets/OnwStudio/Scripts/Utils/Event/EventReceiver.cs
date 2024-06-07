using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

[DisallowMultipleComponent]
public sealed class EventReceiver : MonoBehaviour
{
    private MMF_Player _eventReceiver = null;

    private void Awake()
    {
        _eventReceiver = gameObject.AddComponent<MMF_Player>();
    }

    private void Start()
    {
        _eventReceiver.Events.OnComplete = new();
        _eventReceiver.Events.OnComplete.AddListener(onComplitedEvents);

        _eventReceiver.Events.Initialization();
    }

    private void onComplitedEvents()
    {
#if DEBUG
        Debug.Log("Event End Receiver");
#endif

        _eventReceiver.FeedbacksList.Clear();
    }

    public void PlayEvent(params MMF_Feedback[] feedbacks)
    {
        _eventReceiver.FeedbacksList.AddRange(feedbacks);

        _eventReceiver.Initialization();
        _eventReceiver.PlayFeedbacks();
    }
}
