using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

namespace Onw.Event
{
    using Coroutine = UnityEngine.Coroutine;
    /// <summary>
    /// .. MM 피드백을 사용하는 이벤트 센더 클래스 입니다. 파리미터로 들어온 이벤트 배열을 순차적으로 처리합니다
    /// 이벤트들을 하나로 묶으려면 MMF_Paralle 이벤트 클래스를 참고해주세요
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EventSender : MonoBehaviour
    {
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 true가 되기전에 호출됩니다
        /// </summary>
        [field: SerializeField] public UnityEvent OnStartBeginEvent { get; private set; } = new();
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 true가 된 후 호출됩니다
        /// </summary>
        [field: SerializeField] public UnityEvent OnStartEndEvent { get; private set; } = new();
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 false가 되기 전에 호출됩니다
        /// </summary>
        [field: SerializeField] public UnityEvent OnComplitedBeginEvent { get; private set; } = new();
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 false가 된 후 호출됩니다
        /// </summary>
        [field: SerializeField] public UnityEvent OnComplitedEndEvent { get; private set; } = new();

        private MMF_Player _eventReceiver = null;
        private Coroutine _playCoroutine = null;
        public bool IsPlaying { get; private set; } = false;

        private void Awake()
        {
            _eventReceiver = gameObject.AddComponent<MMF_Player>();
        }

        private void OnDestroy()
        {
            onComplitedEvents();
        }

        private void OnDisable()
        {
            onComplitedEvents();
        }

        /// <summary>
        /// .. 파라미터 배열 형태로 이벤트를 받아 재생합니다
        /// </summary>
        /// <param name="feedbacks"></param>
        public void PlayEvent(params MMF_Feedback[] feedbacks)
        {
            PlayEvents(feedbacks);
        }

        /// <summary>
        /// .. Enumerable 형태의 자료구조를 받아 이벤트를 재생시킵니다
        /// </summary>
        /// <param name="feedbacks"></param>
        public void PlayEvents(IEnumerable<MMF_Feedback> feedbacks)
        {
            OnStartBeginEvent.Invoke();
            IsPlaying = true;
            OnStartEndEvent.Invoke();

            _eventReceiver.StopFeedbacks();
            _eventReceiver.FeedbacksList.Clear();

            if (_playCoroutine is not null) // .. 기존 이벤트 강제종료
            {
                StopCoroutine(_playCoroutine);
            }

            _playCoroutine = StartCoroutine(iEPlayFeedbacks(feedbacks));
        }

        private void onComplitedEvents()
        {
            if (!IsPlaying) return;

#if DEBUG
            Debug.Log("Event Completed!");
#endif
            OnComplitedBeginEvent.Invoke();
            IsPlaying = false;
            OnComplitedEndEvent.Invoke();
        }

        private IEnumerator iEPlayFeedbacks(IEnumerable<MMF_Feedback> feedbacks)
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
}