// using System.Collections.Generic;
// using System.Linq;
// using Onw.Attribute;
// using UnityEngine;
// using UnityEngine.Serialization;
// namespace TMCard.Effect
// {
//     using static ITMCardEffectCreator;
//
//     [SerializeReferenceDropdownName("(특수) 버리기"), Substitution("버리기")]
//     public sealed class DropEffectCreator : ITMCardSpecialEffectCreator
//     {
//         [SerializeReference, DisplayAs("버리기"), FormerlySerializedAs("_cardEffectCreators"), SerializeReferenceDropdown]
//         private List<ITMCardNormalEffectCreator> _cardEffectCreators = new();
//
//         public IEnumerable<ITMNormalEffect> DropEffects => _cardEffectCreators
//             .Select(creator => creator?.CreateEffect())
//             .OfType<ITMNormalEffect>();
//
//         public ITMCardEffect CreateEffect()
//         {
//             return CardEffectGenerator.CreateEffect<DropEffect, DropEffectCreator>(this);
//         }
//     }
// }
