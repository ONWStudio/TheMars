using System;
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
        [field: SerializeField] public UnityEvent OnPlay { get; private set; } = new();
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 false가 된 후 호출됩니다
        /// </summary>
        [field: SerializeField] public UnityEvent OnCompleted { get; private set; } = new();

        [field: SerializeField, InitializeRequireComponent] public MMF_Player EventPlayer { get; private set; } = null;
        [field: SerializeField, ReadOnly] public bool IsPlaying { get; private set; } = false;

        private Coroutine _playCoroutine = null;
        private readonly Queue<IEnumerable<MMF_Feedback>> _eventQueue = new();

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
            this.StopCoroutineIfNotNull(_playCoroutine);
            EventPlayer.StopFeedbacks();
            EventPlayer.FeedbacksList.Clear();
            _eventQueue.Clear();
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
            OnPlay.Invoke();
            IsPlaying = true;

            stopSender();

            _eventQueue.Enqueue(feedbacks);
            _playCoroutine = StartCoroutine(iEPlayFeedbacks());
        }

        public void QueueEvents(IEnumerable<MMF_Feedback> feedbacks)
        {
            if (!IsPlaying)
            {
                PlayEvents(feedbacks);
            }
            else
            {
                _eventQueue.Enqueue(feedbacks);
            }
        }

        public void QueueEvent(params MMF_Feedback[] feedbacks)
        {
            if (!IsPlaying)
            {
                PlayEvents(feedbacks);
            }
            else
            {
                _eventQueue.Enqueue(feedbacks);
            }
        }

        private void onCompletedEvents()
        {
            if (!IsPlaying) return;

            IsPlaying = false;
            OnCompleted.Invoke();
            _playCoroutine = null;
        }

        private IEnumerator iEPlayFeedbacks()
        {
            while (_eventQueue.TryDequeue(out IEnumerable<MMF_Feedback> feedbacks))
            {
                foreach (MMF_Feedback feedback in feedbacks)
                {
                    EventPlayer.AddFeedback(feedback);
                    EventPlayer.Initialization();

                    yield return EventPlayer.PlayFeedbacksCoroutine(transform.position);
                    EventPlayer.FeedbacksList.Clear();
                }
            }

            onCompletedEvents();
        }
    }
}