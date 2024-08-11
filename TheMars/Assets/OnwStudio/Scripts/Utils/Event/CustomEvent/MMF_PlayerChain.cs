using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using JetBrains.Annotations;

namespace Onw.Event
{
    public sealed class MMF_PlayerChain : MMF_Feedback
    {
        [field: SerializeField] public MMF_Player TargetMMFPlayer { get; private set; } = null;

        public override float FeedbackDuration 
        { 
            get => TargetMMFPlayer.TotalDuration; 
            set {} 
        }

        public override Color FeedbackColor => Color.cyan;

        protected override void CustomInitialization(MMF_Player owner)
        {
            TargetMMFPlayer.Initialization();
        }

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            IsPlaying = true;

            if (Active && TargetMMFPlayer != null)
            {
                TargetMMFPlayer.PlayFeedbacks();
            }
        }

        protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            TargetMMFPlayer.StopFeedbacks();
            IsPlaying = false;
        }


        public MMF_PlayerChain(MMF_Player chainPlayer) : base()
        {
            TargetMMFPlayer = chainPlayer;
        }
    }
}