using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class ItmCardCardDropEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardDropEffectCreator>
    {
        public string Description => $"패에서 카드를 {_dropCount}개 버리기";
        
        [SerializeField, ReadOnly]
        private int _dropCount = 1;

        public void Initialize(TMCardDropEffectCreator effectCreator)
        {
            _dropCount = effectCreator.DropCount;
        }

        public void ApplyEffect(TMCardModel model, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState => { });
        }
    }
}