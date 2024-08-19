using Onw.Attribute;
using Onw.Feedback;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class TMCardCopyEffect : ITMNormalEffect, ITMInitializeEffect<TMCardCopyEffectCreator>
    {
        [SerializeField, ReadOnly]
        private TMCardData _copyCardData;
        [SerializeField, ReadOnly]
        private int _copyCount;

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            
            trigger.OnEffectEvent.AddListener(eventState => 
            {
                for (int i = 0; i < _copyCount; i++)
                {
                    TMCardController card = service.CardCreator.CreateCardByCardData(_copyCardData);
                    service.FeedbackPlayer.EnqueueEvent(
                        FeedbackCreator.CreateUnityEvent(() =>
                        {
                            service.CardHandController.AddCard(card);
                            card.transform.localPosition = new(0f, 0f, 0f);
                            service.FeedbackPlayer.EnqueueEventToHead(service.CardHandController.GetSortCardsFeedbacks(0.5f));
                        }),
                        card.GetMoveToScreenCenterEvent(0.5f));
                }
            });
        }

        public void Initialize(TMCardCopyEffectCreator effectCreator)
        {
            _copyCardData = effectCreator.CopyCardData;
            _copyCount = effectCreator.CopyCount;
        }
    }
}