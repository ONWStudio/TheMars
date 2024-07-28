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
    public sealed class DrawEffect : ITMCardSpecialEffect, ITMInitializableEffect<DrawEffectCreator>, IDescriptionList
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("드로우");

        public IEnumerable<string> Descriptions => _drawEffects
            .OfType<IDescriptable>()
            .Select(descriptable => descriptable.Description);

        private readonly List<ITMNormalEffect> _drawEffects = new();

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.DrawEndedState = () =>
            {
                _drawEffects
                    .ForEach(drawEffect => drawEffect.ApplyEffect(cardController));

                TMCardGameManager.Instance.DrawUse(cardController);
            };
        }

        public void Initialize(DrawEffectCreator effectCreator)
        {
            _drawEffects.AddRange(effectCreator.DrawEffects);
        }
    }
}
