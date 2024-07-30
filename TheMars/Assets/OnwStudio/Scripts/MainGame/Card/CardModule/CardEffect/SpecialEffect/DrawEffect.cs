using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;
using Onw.Interface;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 드로우
    /// </summary>
    public sealed class DrawEffect : ITMCardSpecialEffect, ITMCardEffect, ITMInitializableEffect<DrawEffectCreator>, ITMEffectTrigger
    {
        public string Label => "Draw";

        public CardEvent OnEffectEvent { get; } = new();

        private readonly List<ITMNormalEffect> _drawEffects = new();

        public void Initialize(DrawEffectCreator effectCreator)
        {
            _drawEffects.AddRange(effectCreator.DrawEffects);
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            _drawEffects.ForEach(effect => effect.ApplyEffect(controller, this));
            controller.OnDrawEndedEvent.RemoveAllToAddListener(OnEffectEvent.Invoke);
        }
    }
}
