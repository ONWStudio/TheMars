using Onw.Attribute;
using Onw.Feedback;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class ItmCardCardCopyEffect : ITMNormalEffect, ITMCardInitializeEffect<ItmCardCardCopyEffectCreator>
    {
        public string Description => $"'{(_copyCardData ? _copyCardData.CardName : "")}'카드를 {_copyCount}개 획득";

        [SerializeField, ReadOnly]
        private TMCardData _copyCardData;
        [SerializeField, ReadOnly]
        private int _copyCount;

        public void Initialize(ItmCardCardCopyEffectCreator effectCreator)
        {
            _copyCardData = effectCreator.CopyCardData;
            _copyCount = effectCreator.CopyCount;
        }
        
        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            
            trigger.OnEffectEvent.AddListener(eventState =>
            {
                if (!_copyCardData) return;
                
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
    }
}