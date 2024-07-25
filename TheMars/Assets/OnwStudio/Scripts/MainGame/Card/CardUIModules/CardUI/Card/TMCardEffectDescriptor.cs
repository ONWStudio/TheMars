using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent, RequireComponent(typeof(VerticalLayoutGroup))]
    public sealed class TMCardEffectDescriptor : MonoBehaviour
    {
        public IReadOnlyList<RectTransform> Descriptions => _descriptions;

        [Header("Descriptions")]
        [SerializeField, ReadOnly] private List<RectTransform> _descriptions = new();

        public void AddDescriptions(params RectTransform[] descriptionElements)
        {
            _descriptions.AddRange(descriptionElements);
        }
    }
}
