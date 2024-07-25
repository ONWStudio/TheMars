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
        [InspectorButton("Generate GUID")]
        private void generateNewGUID()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        [OnChangedValueByMethod(nameof(_cardEffects))]
        private void onChangedSpecialEffect()
        {
            for (int i = 0; i < _cardEffects.Count; i++)
            {
                if (_cardEffects[i] is not ITMCardSpecialEffect) continue;

                for (int j = i + 1; j < _cardEffects.Count; j++)
                {
                    if (_cardEffects[j] is not ITMCardSpecialEffect || _cardEffects[i]?.GetType().Name != _cardEffects[j]?.GetType().Name) continue;

                    Debug.LogWarning("특수 효과는 같은 효과가 중첩 될 수 없습니다");
                }
            }
        }
    }
}
#endif