#if UNITY_EDITOR
using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TMCard.Effect
{
    public sealed partial class HoldEffectCreator : ITMEffectCreator
    {
        [OnChangedValueByMethod(nameof(FriendlyCard))]
        private void OnChangedFriendlyCard()
        {
            Debug.Log("changed friendlyCard");

            foreach (string guid in AssetDatabase.FindAssets($"t:{typeof(TMCardData).Name}"))
            {
                var cardData = AssetDatabase
                    .LoadAssetAtPath<TMCardData>(AssetDatabase.GUIDToAssetPath(guid));

                var fieldInfo = cardData.GetType().GetField("_effectCreators", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo?.GetValue(cardData) is List<ITMEffectCreator> specialEffects)
                {
                    foreach (var cardSpecialEffect in specialEffects)
                    {
                        if (cardSpecialEffect is not HoldEffectCreator holdCard || holdCard != this || FriendlyCard != cardData) continue;

                        Debug.LogWarning("보유 효과의 카드는 자기 자신을 트리거로 카드로 지정할 수 없습니다");
                        FriendlyCard = null;
                        return;
                    }
                }
            }
        }
    }
}
#endif
