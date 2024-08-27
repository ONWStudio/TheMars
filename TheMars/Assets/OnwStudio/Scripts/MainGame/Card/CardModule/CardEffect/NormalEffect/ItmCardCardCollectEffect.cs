using Onw.Attribute;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect
{
    /// <summary>
    /// .. TODO : 카드 획득 효과 수정 카드 선택해서 버리기 
    /// </summary>
    public sealed class ItmCardCardCollectEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardCollectEffectCreator>
    {
        public string Description => $"랜덤 카드를 선택해서 획득";
        
        [SerializeField, ReadOnly]
        private int _collectCount;
        [SerializeField, ReadOnly]
        private TMCardKind _selectKind = TMCardKind.NONE;

        public void Initialize(TMCardCollectEffectCreator effectCreator)
        {
            _collectCount = effectCreator.CollectCount;
            _selectKind = effectCreator.SelectKind;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState 
                => collectCard(controller, _collectCount));
        }
        
        private static void collectCard(TMCardController triggerCard, int collectCount)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            
            service.CollectUI.ActiveUI();
        }

    }
}