using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;

namespace TMCard.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(VerticalLayoutGroup))]
    public sealed class TMCardDescriptor : MonoBehaviour
    {
        public IReadOnlyList<TMCardEffectDescriptor> Descriptors => _descriptors;

        [SerializeField, ReadOnly] private List<TMCardEffectDescriptor> _descriptors = new();

        public void SetDescription(TMCardData cardData)
        {
            
        }
    }
}
