using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Interface;
using TMCard.Runtime;
using TMCard.Effect;

namespace TMCard.Effect
{
    using Runtime;
    using Effect;

    /// <summary>
    /// .. 버리기
    /// </summary>
    public sealed class DropEffect : ITMCardSpecialEffect, ITMInitializableEffect<DropEffectCreator>, IDescriptionList
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("버리기");
        public IEnumerable<string> Descriptions =>
            _dropEffect
            .OfType<IDescriptable>()
            .Select(descriptable => descriptable.Description);

        private readonly List<ITMNormalEffect> _dropEffect = new();

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.TurnEndedState = () =>
            {
                _dropEffect.ForEach(cardEffect => cardEffect.ApplyEffect(cardController));
                TMCardGameManager.Instance.DisposeCard(cardController);
            };
        }

        public void Initialize(DropEffectCreator effectCreator)
        {
            _dropEffect.AddRange(effectCreator.DropEffects);
        }
    }
}