using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineExtensions;
using MoreMountains.Feel;
using MoreMountains.Feedbacks;

public sealed class FeedbackParallel : MMF_Feedback
{
    [field: SerializeField] public List<MMF_Feedback> Feedbacks { get; } = new();

    private Coroutine _playedFeedbackCoroutine = null;

    public override float FeedbackDuration
    {
        get => Feedbacks.Sum(feedback => feedback.TotalDuration);
        set { }
    }

    protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
    {
        IsPlaying = true;
        _playedFeedbackCoroutine = Owner.StartCoroutine(iEPlayAllFeedback(position, feedbacksIntensity));
    }

    protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
    {
        if (_playedFeedbackCoroutine is not null)
        {
            Owner.StopCoroutine(_playedFeedbackCoroutine);
        }

        Feedbacks
            .ForEach(feedback => feedback.Stop(position, feedbacksIntensity));
    }

    protected override void CustomReset()
    {
        if (_playedFeedbackCoroutine is not null)
        {
            Owner.StopCoroutine(_playedFeedbackCoroutine);
        }
    }

    private IEnumerator iEPlayAllFeedback(Vector3 position, float feedbacksIntensity)
    {
        IEnumerable<Coroutine> coroutines = Feedbacks
            .Select(feedback => Owner.StartCoroutine(iEPlayFeedback(feedback, position, feedbacksIntensity)));

        foreach (Coroutine coroutine in coroutines)
        {
            yield return coroutine;
        }

        IsPlaying = false;
    }

    private IEnumerator iEPlayFeedback(MMF_Feedback feedback, Vector3 position, float feedbacksIntensity)
    {
        feedback.Play(position, feedbacksIntensity);
        yield return CoroutineHelper.WaitForSeconds(feedback.TotalDuration);
    }
}
