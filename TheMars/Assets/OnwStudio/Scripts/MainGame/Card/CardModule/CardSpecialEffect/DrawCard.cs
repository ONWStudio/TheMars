using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;
using Onw.Interface;
using TMCard.UI;
using TMCard.Effect;

namespace TMCard.SpecialEffect
{
    /// <summary>
    /// .. 드로우
    /// </summary>
    [SerializeReferenceDropdownName("드로우")]
    public sealed class DrawCard : ITMCardSpecialEffect, IDescriptable
    {
        public int No => 11;
        public string Description => _drawEffects
            .OfType<IDescriptable>()
            .BuildToString();

        [SerializeReference, DisplayAs("드로우 효과"), FormerlySerializedAs("_drawEffects"), SerializeReferenceDropdown] private List<ITMCardEffect> _drawEffects = new();

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.DrawEndedState = () =>
            {
                _drawEffects
                    .ForEach(drawEffect => drawEffect.OnEffect(cardController.CardData));

                TMCardGameManager.Instance.DrawUse(cardController);
            };
        }
    }
}
