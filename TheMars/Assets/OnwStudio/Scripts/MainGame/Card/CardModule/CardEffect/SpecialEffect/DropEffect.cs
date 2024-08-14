using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Interface;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 버리기
    /// </summary>
    public sealed class DropEffect : TMCardSpecialEffect, ITMInitializableEffect<DropEffectCreator>, ITMEffectTrigger
    {
        public CardEvent OnEffectEvent { get; } = new();

        private readonly List<ITMNormalEffect> _dropEffect = new();

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            _dropEffect.ForEach(effect => effect.ApplyEffect(controller, this));
            controller.OnTurnEndedEvent.RemoveAllToAddListener(() =>
            {
                OnEffectEvent.Invoke();
                TMCardHelper.Instance.DisposeCard(controller);
            });
        }

        public void Initialize(DropEffectCreator effectCreator)
        {
            _dropEffect.AddRange(effectCreator.DropEffects);
        }

        public DropEffect() : base("Drop") {}
    }
}