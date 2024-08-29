// using Onw.Attribute;
// using UnityEngine;
// namespace TMCard.Effect
// {
//     using static ITMCardEffectCreator;
//
//     [SerializeReferenceDropdownName("(특수) 지속 (시간)"), Substitution("지속(시간)")]
//     public class TimeContinuingEffectCreator : ITMCardSpecialEffectCreator
//     {
//         [field: SerializeField, DisplayAs("지속 시간")] public float ContinuingTime { get; private set; } = 1f;
//
//         public ITMCardEffect CreateEffect()
//         {
//             return CardEffectGenerator.CreateEffect<TimeContinuingEffect, TimeContinuingEffectCreator>(this);
//         }
//     }
// }
