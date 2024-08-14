#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
using TMCard.Effect;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        [OnChangedValueByMethod(nameof(_effectCreators))]
        private void OnChangedSpecialEffect()
        {
            for (int i = 0; i < _effectCreators.Count; i++)
            {
                if (_effectCreators[i] is not ITMSpecialEffectCreator) continue;

                for (int j = i + 1; j < _effectCreators.Count; j++)
                {
                    if (_effectCreators[j] is not ITMSpecialEffectCreator || _effectCreators[i]?.GetType().Name != _effectCreators[j]?.GetType().Name) continue;

                    Debug.LogWarning("특수 효과는 같은 효과가 중첩 될 수 없습니다");
                }
            }
        }
    }
}
#endif