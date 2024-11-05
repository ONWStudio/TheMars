using UnityEngine;
using Onw.Attribute;
using Onw.Interface;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;
using UnityEngine.Localization;

namespace TM.Card.Effect
{
    public sealed class TMCardCollectEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardCollectEffectCreator>
    {
        [field: SerializeField] public LocalizedString LocalizedDescription { get; private set; }

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
            trigger.OnEffectEvent += _ 
                => collectCard(cardModel, _collectCount);
        }
        
        private static void collectCard(TMCardModel triggerCard, int collectCount)
        {
            // service.CollectUI.ActiveUI();
        }

        public void Dispose() {}
    }
}