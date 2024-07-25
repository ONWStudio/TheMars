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
    [SerializeReferenceDropdownName("버리기"), Substitution("버리기")]
    public sealed class DropCard : ITMCardSpecialEffect, IDescriptionList
    {
        public string Label => throw new System.NotImplementedException();
        public IEnumerable<string> Descriptions =>
            _cardEffects
            .OfType<IDescriptable>()
            .Select(descriptable => descriptable.Description);

        [SerializeReference, DisplayAs("버리기 효과"), SerializeReferenceDropdown] private List<ITMNormalEffect> _cardEffects = new();

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.TurnEndedState = () =>
            {
                _cardEffects.ForEach(cardEffect => cardEffect.ApplyEffect(cardController));
                TMCardGameManager.Instance.DisposeCard(cardController);
            };
        }
    }
}