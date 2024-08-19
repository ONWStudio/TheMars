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

    public interface IQueueableFeedbackPlayer
    {
        void EnqueueEvents(List<MMF_Feedback> feedbacks);
        void EnqueueEvent(params MMF_Feedback[] feedbacks);
        void EnqueueEventsToHead(List<MMF_Feedback> feedbacks);
        void EnqueueEventToHead(params MMF_Feedback[] feedbacks);
    }

    public interface IStopFeedbackPlayer
    {
        void StopNotifyDisable();
        void StopToNotify();
    }

    public interface IPlayableFeedbackPlayer
    {
        void PlayEvents();
    }
    
    public interface IEventFeedbackPlayer
    {
        SafeUnityEvent OnPlay { get; }
        SafeUnityEvent OnCompleted { get; }
        SafeUnityEvent<int> OnAddedFeedback { get; }
    }

    public interface IFeedbackPlayerOption
    {
        MMF_Player EventPlayer { get; }
        bool IsPlaying { get; }
    }
    
    public interface IIgnorePlayFeedbackPlayer : IQueueableFeedbackPlayer, IStopFeedbackPlayer, IEventFeedbackPlayer, IFeedbackPlayerOption {}

    /// <summary>
    /// .. MM 피드백을 사용하는 이벤트 센더 클래스 입니다. 파리미터로 들어온 이벤트 배열을 순차적으로 처리합니다
    /// 이벤트들을 하나로 묶으려면 MMF_Parallel 이벤트 클래스를 참고해주세요
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FeedbackPlayer : MonoBehaviour, IIgnorePlayFeedbackPlayer, IPlayableFeedbackPlayer
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
            
            IsPlaying = true;
            
            OnPlay.Invoke();
            _playCoroutine = StartCoroutine(iEPlayFeedbacks());
        }

        public void EnqueueEvents(List<MMF_Feedback> feedbacks)
        {
            EnqueueEvent(feedbacks.ToArray());
        }

        public void EnqueueEvent(params MMF_Feedback[] feedbacks)
        {
            _eventQueue.AddLast(feedbacks);
            OnAddedFeedback.Invoke(feedbacks.Length);
        }

        public void EnqueueEventsToHead(List<MMF_Feedback> feedbacks)
        {
            EnqueueEventToHead(feedbacks.ToArray());
        }

        public void EnqueueEventToHead(params MMF_Feedback[] feedbacks)
        {
            _eventQueue.AddFirst(feedbacks);
            OnAddedFeedback.Invoke(feedbacks.Length);
        }

        private void onCompletedEvents()
        {
            if (!IsPlaying) return;

            IsPlaying = false;
            OnCompleted.Invoke();
            _eventQueue.Clear();
            _playCoroutine = null;
        }

        private IEnumerator iEPlayFeedbacks()
        {
            while (_eventQueue.TryDequeueFirst(out MMF_Feedback[] feedbacks))
            {
                foreach (MMF_Feedback feedback in feedbacks)
                {
                    EventPlayer.AddFeedback(feedback);
                    EventPlayer.Initialization();

                    yield return EventPlayer.PlayFeedbacksCoroutine(transform.position);
                    EventPlayer.FeedbacksList.Clear();
                }

                yield return null; // .. 프레임 보간 이벤트가 바로 들어오는 경우
            }

            onCompletedEvents();
        }
    }
}