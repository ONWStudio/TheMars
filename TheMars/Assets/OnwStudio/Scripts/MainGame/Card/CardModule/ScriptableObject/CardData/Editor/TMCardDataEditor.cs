#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        [InspectorButton("Generate GUID")]
        private void generateNewGUID()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        [OnValueChangedByMethod(nameof(_specialEffect))]
        private void onChangedSpecialEffect()
        {
            for (int i = 0; i < _specialEffect.Count - 1; i++)
            {
                if (_specialEffect[^1]?.GetType().Name != _specialEffect[i]?.GetType().Name) continue;

                Debug.LogWarning("특수 효과는 같은 효과가 중첩 될 수 없습니다");
                _specialEffect.RemoveAt(_specialEffect.Count - 1);
            }
        }
    }
}
#endif