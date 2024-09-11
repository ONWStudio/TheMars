#if UNITY_EDITOR
using UnityEngine;
using Onw.Event;

namespace TM.Card
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