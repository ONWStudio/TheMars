using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace Onw.Feedback
{
    using Coroutine;
    using Coroutine = UnityEngine.Coroutine;

    public sealed class MMF_Parallel : MMF_Feedback
    {
        [field: SerializeField] public List<MMF_Feedback> Feedbacks { get; private set; } = new();

        private Coroutine _playedFeedbackCoroutine = null;

        public override float FeedbackDuration
        {
            get => Feedbacks.Count > 0 ? Feedbacks.Max(feedback => feedback.FeedbackDuration) : 0;
            set { }
        }

        protected override void CustomInitialization(MMF_Player owner)
        {
            for (int i = 0; i < Feedbacks.Count; i++)
            {
                Feedbacks[i].Initialization(owner, i);
            }
        }

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            IsPlaying = true;

            _playedFeedbackCoroutine = Owner
                .StartCoroutine(iEPlayAllFeedback(position, feedbacksIntensity));

            IEnumerator iEPlayAllFeedback(Vector3 position, float feedbacksIntensity)
            {
                Feedbacks
                    .ForEach(feedback => feedback.Play(position, feedbacksIntensity));

                yield return CoroutineHelper.WaitForSeconds(FeedbackDuration);
                IsPlaying = false;
            }
        }

        protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            Owner.StopCoroutineIfNotNull(_playedFeedbackCoroutine);

            Feedbacks
                .ForEach(feedback => feedback.Stop(position, feedbacksIntensity));
        }

        protected override void CustomReset()
        {
            Owner.StopCoroutineIfNotNull(_playedFeedbackCoroutine);
        }
    }
}