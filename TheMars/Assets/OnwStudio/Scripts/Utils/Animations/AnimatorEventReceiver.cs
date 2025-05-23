using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace Onw.Animation
{
    using Onw.Coroutine;
    using Coroutine = UnityEngine.Coroutine;

    public static class AnimatorEventExtension
    {
        private static AnimatorEventReceiver AttachReceiever(Animator animator)
            => animator.gameObject.TryGetComponent<AnimatorEventReceiver>(out AnimatorEventReceiver receiever) ? receiever : animator.gameObject.AddComponent<AnimatorEventReceiver>();

        public static void SetInteger(this Animator animator, string name, int value, Action onFinished)
        {
            animator.SetInteger(name, value);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetInteger(this Animator animator, int id, int value, Action onFinished)
        {
            animator.SetInteger(id, value);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetFloat(this Animator animator, string name, float value, Action onFinished)
        {
            animator.SetFloat(name, value);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetFloat(this Animator animator, int id, float value, Action onFinished)
        {
            animator.SetFloat(id, value);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetBool(this Animator animator, string name, bool value, Action onFinished)
        {
            animator.SetBool(name, value);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetBool(this Animator animator, int id, bool value, Action onFinished)
        {
            animator.SetBool(id, value);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetTrigger(this Animator animator, string name, Action onFinished)
        {
            animator.SetTrigger(name);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }

        public static void SetTrigger(this Animator animator, int hashid, Action onFinished)
        {
            animator.SetTrigger(hashid);
            AttachReceiever(animator).OnStateEnd(onFinished);
        }
    }

    [RequireComponent(typeof(Animator))]
    public class AnimatorEventReceiver : MonoBehaviour
    {
        #region Inspector

        public List<AnimationClip> animationClips = new List<AnimationClip>();

        #endregion

        private Animator _animator = null;
        private readonly Dictionary<string, List<Action>> _startEvnets = new Dictionary<string, List<Action>>();
        private readonly Dictionary<string, List<Action>> _endEvents = new Dictionary<string, List<Action>>();

        private Coroutine _coroutine = null;
        private bool _isPlayingAnimator = false;

        public void OnStateEnd(Action onFinished)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(OnStateEndCheck(onFinished));
        }

        public IEnumerator OnStateEndCheck(Action onFinished)
        {
            _isPlayingAnimator = true;
            while (true)
            {
                yield return CoroutineHelper.WaitForEndOfFrame;
                if (!_isPlayingAnimator)
                {
                    // 다음 애니메이션 클립이 재생되는지 1프레임 더 기다림
                    yield return CoroutineHelper.WaitForEndOfFrame;
                    if (!_isPlayingAnimator) break;
                }
            }
            onFinished?.Invoke();
        }

        private void Awake()
        {
            // 애니메이터 내에 있는 모든 애니메이션 클립의 시작과 끝에 이벤트를 생성한다.
            _animator = GetComponent<Animator>();
            for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip clip = _animator.runtimeAnimatorController.animationClips[i];
                animationClips.Add(clip);

                AnimationEvent animationStartEvent = new AnimationEvent();
                animationStartEvent.time = 0;
                animationStartEvent.functionName = "AnimationStartHandler";
                animationStartEvent.stringParameter = clip.name;
                clip.AddEvent(animationStartEvent);

                AnimationEvent animationEndEvent = new AnimationEvent();
                animationEndEvent.time = clip.length;
                animationEndEvent.functionName = "AnimationEndHandler";
                animationEndEvent.stringParameter = clip.name;
                clip.AddEvent(animationEndEvent);
            }
        }

        /// <summary>
        /// 각 클립 별 시작 이벤트
        /// </summary>
        /// <param name="name"></param>
        private void AnimationStartHandler(string name)
        {
            if (_startEvnets.TryGetValue(name, out List<Action> actions))
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i]?.Invoke();
                }
                actions.Clear();
            }
            _isPlayingAnimator = true;
        }

        /// <summary>
        /// 클립 별 종료 이벤트
        /// </summary>
        /// <param name="name"></param>
        private void AnimationEndHandler(string name)
        {
            if (_endEvents.TryGetValue(name, out List<Action> actions))
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i]?.Invoke();
                }
                actions.Clear();
            }
            _isPlayingAnimator = false;
        }

        //    public void PlayFeedbacksName(string _name)
        //    {
        //        if (FeedbacksList == null)
        //        {
        //#if DEBUG
        //            Debug.LogError("FeedbacksList Is Null");
        //#endif
        //            return;
        //        }

        //        MMFeedbacks feedbacks = FeedbacksList.GetFeedbacks(_name);
        //        if (feedbacks == null)
        //        {
        //#if DEBUG
        //            Debug.LogError("Not Found Feedbacks");
        //#endif
        //        }
        //        else
        //        {
        //            feedbacks.PlayFeedbacks();
        //        }
        //    }
    }
}