using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class ItmCardCardDropEffect : ITMNormalEffect, ITMCardInitializeEffect<ItmCardCardDropEffectCreator>
    {
        public string Description => $"패에서 카드를 {_dropCount}개 버리기";
        
        [SerializeField, ReadOnly]
        private int _dropCount = 1;

        public void Initialize(ItmCardCardDropEffectCreator effectCreator)
        {
            _dropCount = effectCreator.DropCount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState => { });
        }
    }
}