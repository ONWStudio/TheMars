using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    using UI;
    using Effect;

    /// <summary>
    /// .. 드로우
    /// </summary>
    [SerializeReferenceDropdownName("드로우")]
    public sealed class DrawCard : ITMCardSpecialEffect
    {
        public int No => 11;

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
