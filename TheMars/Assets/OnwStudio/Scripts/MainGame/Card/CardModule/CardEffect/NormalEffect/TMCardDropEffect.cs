using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public sealed class TMCardDropEffect : ITMNormalEffect, ITMInitializableEffect<TMCardDropEffectCreator>
    {
        [SerializeField, ReadOnly]
        private int _dropCount = 1;

        public void Initialize(TMCardDropEffectCreator effectCreator)
        {
            _dropCount = effectCreator.DropCount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() => { });
        }
    }
}