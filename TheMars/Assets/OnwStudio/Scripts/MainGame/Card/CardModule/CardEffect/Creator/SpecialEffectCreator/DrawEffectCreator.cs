// using System.Collections.Generic;
// using System.Linq;
// using Onw.Attribute;
// using UnityEngine;
// using UnityEngine.Serialization;
// namespace TMCard.Effect
// {
//     using static ITMCardEffectCreator;
//
//     [SerializeReferenceDropdownName("(특수) 드로우")]
//     public sealed class DrawEffectCreator : ITMCardSpecialEffectCreator
//     {
//         [SerializeReference, DisplayAs("드로우 효과"), FormerlySerializedAs("_drawEffectCreators"), SerializeReferenceDropdown]
//         private List<ITMCardNormalEffectCreator> _drawEffectCreators = new();
//
//         public IEnumerable<ITMNormalEffect> DrawEffects => _drawEffectCreators
//             .Select(creator => creator.CreateEffect())
//             .OfType<ITMNormalEffect>();
//
//         public ITMCardEffect CreateEffect()
//         {
//             return CardEffectGenerator.CreateEffect<DrawEffect, DrawEffectCreator>(this);
//         }
//     }
// }
