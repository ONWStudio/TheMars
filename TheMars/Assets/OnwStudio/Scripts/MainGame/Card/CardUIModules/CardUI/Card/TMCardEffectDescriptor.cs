// using System.Collections.Generic;
// using Onw.Attribute;
// using UnityEngine;
// using UnityEngine.Serialization;
// using UnityEngine.UI;
// namespace TM.Runtime
// {
//     [DisallowMultipleComponent, RequireComponent(typeof(VerticalLayoutGroup))]
//     public sealed class TMCardEffectDescriptor : MonoBehaviour
//     {
//         public IReadOnlyList<RectTransform> Descriptions => _descriptions;
//
//         [FormerlySerializedAs("descriptions")]
//         [Header("Descriptions")]
//         [SerializeField, ReadOnly]
//         private List<RectTransform> _descriptions = new();
//
//         public void AddDescriptions(params RectTransform[] descriptionElements)
//         {
//             _descriptions.AddRange(descriptionElements);
//         }
//     }
// }
