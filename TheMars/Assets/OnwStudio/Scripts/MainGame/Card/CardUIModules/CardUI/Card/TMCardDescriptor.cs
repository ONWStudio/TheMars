// using System.Collections.Generic;
// using Onw.Attribute;
// using UnityEngine;
// using UnityEngine.Serialization;
// using UnityEngine.UI;
// namespace TM.Runtime
// {
//     [DisallowMultipleComponent]
//     public sealed class TMCardDescriptor : MonoBehaviour
//     {
//         public IReadOnlyList<RectTransform> Descriptors => _descriptors;
//
//         [FormerlySerializedAs("descriptors")]
//         [SerializeField, ReadOnly]
//         private List<RectTransform> _descriptors = new();
//
//         [FormerlySerializedAs("descriptionArea")]
//         [SerializeField, InitializeRequireComponent]
//         private RectTransform _descriptionArea;
//         [FormerlySerializedAs("verticalLayoutGroup")]
//         [SerializeField, InitializeRequireComponent]
//         private VerticalLayoutGroup _verticalLayoutGroup;
//
//         public void SetDescription(TMCardData cardData)
//         {
//             //foreach (string specialEffectTypeName in cardData.SpecialEffectTypeNames)
//             //{
//             //    StringBuilder stringBuilder = new();
//             //    stringBuilder.Append(cardData.GetSpecialEffectName(specialEffectTypeName));
//
//             //    TextMeshProUGUI tmpText = UIHelper
//             //        .GetNewUIObject(transform, specialEffectTypeName)
//             //        .gameObject
//             //        .AddComponent<TextMeshProUGUI>();
//
//             //    tmpText.alignment = TextAlignmentOptions.Center;
//             //    tmpText.text = stringBuilder.ToString();
//             //}
//
//             //foreach (ITMCardEffect specialEffect in cardData.CardEffects)
//             //{
//             //    if (specialEffect is TMCardResourceEffect resourceEffect)
//             //    {
//             //        StringBuilder stringBuilder = new();
//             //        stringBuilder.Append(resourceEffect.Amount > 0 ? "+" : "-");
//             //        stringBuilder.Append(resourceEffect switch
//             //        {
//             //            MarsLithumEffect marsLithumEffect => "마르스리튬",
//             //            TeraResourceEffect teraResource => "테라",
//             //            _ => "[Null]"
//             //        });
//
//             //        TextMeshProUGUI tmpText = UIHelper
//             //            .GetNewUIObject(transform, specialEffect.GetType().Name)
//             //            .gameObject
//             //            .AddComponent<TextMeshProUGUI>();
//
//             //        tmpText.text = stringBuilder.ToString();
//             //    }
//             //}
//         }
//     }
// }
