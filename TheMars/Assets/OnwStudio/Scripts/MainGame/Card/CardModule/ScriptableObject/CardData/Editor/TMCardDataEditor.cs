#if UNITY_EDITOR
using System;
using Onw.Attribute;
using Onw.Event;
using TMCard.Effect;
using UnityEngine;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        private readonly SafeAction _onValueChanged = new();

        private void OnValidate()
        {
            _onValueChanged?.Invoke();
        }
    }
}
#endif