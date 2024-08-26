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
        
        [OnChangedValueByMethod(nameof(_effectCreators))]
        private void OnChangedSpecialEffect()
        {
            for (int i = 0; i < _effectCreators.Count; i++)
            {
                if (_effectCreators[i] is not ITMCardSpecialEffectCreator) continue;

                for (int j = i + 1; j < _effectCreators.Count; j++)
                {
                    if (_effectCreators[j] is not ITMCardSpecialEffectCreator || _effectCreators[i]?.GetType().Name != _effectCreators[j]?.GetType().Name) continue;

                    Debug.LogWarning("특수 효과는 같은 효과가 중첩 될 수 없습니다");
                }
            }
        }

        private void OnValidate()
        {
            _onValueChanged?.Invoke();
        }
    }
}
#endif