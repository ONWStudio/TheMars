using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using OnwAttributeExtensions;

/// <summary>
/// .. 드로우
/// </summary>
[SerializeReferenceDropdownName("드로우")]
public sealed class DrawCard : ICardSpecialEffect
{
    [SerializeReference, DisplayAs("드로우 효과"), FormerlySerializedAs("_drawEffects"), SerializeReferenceDropdown] private List<ICardEffect> _drawEffects = new();

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.DrawEndedState = () =>
        {
            cardController.OnDrawUse.Invoke(cardController);
            _drawEffects
                .ForEach(drawEffect => drawEffect.OnEffect(cardController.gameObject, cardController.CardData));
        };
    }
}
