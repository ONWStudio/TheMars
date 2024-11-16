//using UnityEngine;
//using Onw.Attribute;
//using Onw.Interface;
//using TM.Card.Runtime;
//using TM.Card.Effect.Creator;
//using UnityEngine.Localization;

//namespace TM.Card.Effect
//{
//    public sealed class TMCardCollectEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardCollectEffectCreator>
//    {
//        [field: SerializeField] public LocalizedString LocalizedDescription { get; private set; }

//        public bool CanUseEffect => throw new System.NotImplementedException();

//        [SerializeField, ReadOnly]
//        private int _collectCount;
        
//        [SerializeField, ReadOnly]
//        private TMCardKind _selectKind = TMCardKind.EFFECT;
        
//        public void Initialize(TMCardCollectEffectCreator effectCreator)
//        {
//            _collectCount = effectCreator.CollectCount;
//            _selectKind = effectCreator.SelectKind;
//        }

        
//        private static void collectCard(TMCardModel triggerCard, int collectCount)
//        {
//            // service.CollectUI.ActiveUI();
//        }

//        public void Dispose() {}

//        public void ApplyEffect(TMCardModel cardModel)
//        {
//        }

//        public void OnEffect(TMCardModel cardModel)
//        {
//            collectCard(cardModel, _collectCount);
//        }
//    }
//}