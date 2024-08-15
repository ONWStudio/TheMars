using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class TMCardCopyEffect : ITMNormalEffect, ITMInitializableEffect<TMCardCopyEffectCreator>
    {
        [SerializeField, ReadOnly]
        private TMCardData _copyCardData;
        [SerializeField, ReadOnly]
        private int _copyCount;

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() => 
            {
                Debug.Log("카드 카피");
            });
        }

        public void Initialize(TMCardCopyEffectCreator effectCreator)
        {
            _copyCardData = effectCreator.CopyCardData;
            _copyCount = effectCreator.CopyCount;
        }
    }
}