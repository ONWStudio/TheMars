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
    [SerializeReferenceDropdownName("드로우"), Substitution("드로우")]
    public sealed class DrawCard : ITMCardSpecialEffect, IDescriptionList
    {
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("드로우");

        public IEnumerable<string> Descriptions => _drawEffects
            .OfType<IDescriptable>()
            .Select(descriptable => descriptable.Description);

        [SerializeReference, DisplayAs("드로우 효과"), FormerlySerializedAs("_drawEffects"), SerializeReferenceDropdown] private List<ITMNormalEffect> _drawEffects = new();

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.DrawEndedState = () =>
            {
                _drawEffects
                    .ForEach(drawEffect => drawEffect.ApplyEffect(cardController));

                TMCardGameManager.Instance.DrawUse(cardController);
            };
        }
    }
}
