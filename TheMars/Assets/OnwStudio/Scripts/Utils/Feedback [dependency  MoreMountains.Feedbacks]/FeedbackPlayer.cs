using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Onw.Collections;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Event;

namespace Onw.Feedback
{
    using Coroutine = UnityEngine.Coroutine;
    /// <summary>
    /// .. MM 피드백을 사용하는 이벤트 센더 클래스 입니다. 파리미터로 들어온 이벤트 배열을 순차적으로 처리합니다
    /// 이벤트들을 하나로 묶으려면 MMF_Parallel 이벤트 클래스를 참고해주세요
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FeedbackPlayer : MonoBehaviour
    {
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 true가 되기전에 호출됩니다
        /// </summary>
        [field: SerializeField] public SafeUnityEvent OnPlay { get; private set; } = new();
        /// <summary>
        /// .. 이벤트 메서드 호출 시 IsPlaying이 false가 된 후 호출됩니다
        /// </summary>
        [field: SerializeField] public SafeUnityEvent OnCompleted { get; private set; } = new();
        [field: SerializeField] public SafeUnityEvent<int> OnAddedFeedback { get; private set; } = new();

        [field: SerializeField, InitializeRequireComponent] public MMF_Player EventPlayer { get; private set; } = null;
        [field: SerializeField, ReadOnly] public bool IsPlaying { get; private set; } = false;

        private Coroutine _playCoroutine = null;
        private readonly Deque<MMF_Feedback[]> _eventQueue = new();

        private void OnDestroy()
        {
            StopToNotify();
        }

        private void OnDisable()
        {
            StopToNotify();
        }

        public void StopToNotify()
        {
            StopNotifyDisable();
            onCompletedEvents();
        }

        public void StopNotifyDisable()
        {
            this.StopCoroutineIfNotNull(_playCoroutine);
            EventPlayer.StopFeedbacks();
            EventPlayer.FeedbacksList.Clear();
            _eventQueue.Clear();
        }

        public void PlayEvents()
        {
            if (IsPlaying) return;
            
            OnPlay.Invoke();
            IsPlaying = true;

            _playCoroutine = StartCoroutine(iEPlayFeedbacks());
        }

        public void QueueEvents(List<MMF_Feedback> feedbacks)
        {
            QueueEvent(feedbacks.ToArray());
        }

        public void QueueEvent(params MMF_Feedback[] feedbacks)
        {
            OnAddedFeedback.Invoke(feedbacks.Length);
            _eventQueue.AddLast(feedbacks);
        }

        public void QueueEventsToHead(List<MMF_Feedback> feedbacks)
        {
            QueueEventToHead(feedbacks.ToArray());
        }

        public void QueueEventToHead(params MMF_Feedback[] feedbacks)
        {
            OnAddedFeedback.Invoke(feedbacks.Length);
            _eventQueue.AddFirst(feedbacks);
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
            while (_eventQueue.TryDequeueFirst(out MMF_Feedback[] feedbacks))
            {
                foreach (var feedback in feedbacks)
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