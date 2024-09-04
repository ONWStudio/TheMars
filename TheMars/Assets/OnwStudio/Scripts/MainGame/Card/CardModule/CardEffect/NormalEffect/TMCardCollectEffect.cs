using Onw.Attribute;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect
{
    public sealed class TMCardCollectEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardCollectEffectCreator>
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

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() 
                => collectCard(cardModel, _collectCount));
        }
        
        private static void collectCard(TMCardModel triggerCard, int collectCount)
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager service)) return;
            
            // service.CollectUI.ActiveUI();
        }

        public void Dispose() {}
    }
}