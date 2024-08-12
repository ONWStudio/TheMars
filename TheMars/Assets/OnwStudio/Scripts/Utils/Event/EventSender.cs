using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using Onw.Attribute;
using Onw.Coroutine;

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
        [field: SerializeField] public UnityEvent OnCompletedBeginEvent { get; private set; } = new();
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 false가 된 후 호출됩니다
        /// </summary>
        [field: SerializeField] public UnityEvent OnCompletedEndEvent { get; private set; } = new();

        [field: SerializeField, InitializeRequireComponent] public MMF_Player EventPlayer { get; private set; } = null;
        [field: SerializeField, ReadOnly] public bool IsPlaying { get; private set; } = false;

        private Coroutine _playCoroutine = null;

        private void OnDestroy()
        {
            stopSenderAndDisable();
        }

        private void OnDisable()
        {
            stopSenderAndDisable();
        }

        private void stopSenderAndDisable()
        {
            stopSender();
            onCompletedEvents();
        }

        private void stopSender()
        {
            EventPlayer.StopFeedbacks();
            EventPlayer.FeedbacksList.Clear();
            this.StopCoroutineIfNotNull(_playCoroutine);
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

            stopSender();

            _playCoroutine = StartCoroutine(iEPlayFeedbacks(feedbacks));
        }

        private void onCompletedEvents()
        {
            if (!IsPlaying) return;

            OnCompletedBeginEvent.Invoke();
            IsPlaying = false;
            OnCompletedEndEvent.Invoke();
        }

        private IEnumerator iEPlayFeedbacks(IEnumerable<MMF_Feedback> feedbacks)
        {
            foreach (MMF_Feedback feedback in feedbacks)
            {
                EventPlayer.AddFeedback(feedback);
                EventPlayer.Initialization();

                yield return EventPlayer.PlayFeedbacksCoroutine(transform.position);
                EventPlayer.FeedbacksList.Clear();
            }

            onCompletedEvents();
        }
    }
}