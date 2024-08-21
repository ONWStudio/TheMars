using System.Collections.Generic;
using System.Linq;
using Onw.Feedback;
using Onw.ServiceLocator;
using TMCard.Runtime;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 버리기
    /// </summary>
    public sealed class DropEffect : TMCardSpecialEffect, ITMInitializeEffect<DropEffectCreator>, ITMEffectTrigger, ITMInnerEffector
    {
        public CardEvent OnEffectEvent { get; } = new();

        public IReadOnlyList<ITMNormalEffect> InnerEffect => _dropEffect;

        private readonly List<ITMNormalEffect> _dropEffect = new();

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            
            _dropEffect.ForEach(effect => effect.ApplyEffect(controller, this));
            controller.OnTurnEndedEvent.RemoveAllToAddListener(eventState =>
            {
                MMF_Parallel parallel = controller.GetMoveToUp();
                parallel.Feedbacks.Add(service.CardHandController.RemoveCardToGetSortFeedbacks(controller));
                
                service.FeedbackPlayer.EnqueueEvent(
                    parallel,
                    FeedbackCreator.CreateUnityEvent(() =>
                    {
                        OnEffectEvent.Invoke(eventState);
                        service.FeedbackPlayer.EnqueueEventToHead(
                            controller.GetMoveToScreenCenterEvent(),
                            controller.GetMoveToTombEvent(),
                            FeedbackCreator.CreateUnityEvent(() => service.CardTombController.EnqueueDeadCard(controller)));
                    }));
            });
        }

        public void Initialize(DropEffectCreator effectCreator)
        {
            _dropEffect.AddRange(effectCreator.DropEffects);
        }

        public DropEffect() : base("Drop") {}

    }
}