#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;

namespace TMCard.Effect
{
    public sealed partial class HoldEffect : ITMCardSpecialEffect
    {
        [OnChangedValueByMethod(nameof(_friendlyCard))]
        private void onChangedFriendlyCard()
        {
            Debug.Log("changed friendlyCard");

            foreach (string guid in AssetDatabase.FindAssets($"t:{typeof(TMCardData).Name}"))
            {
                TMCardData cardData = AssetDatabase
                    .LoadAssetAtPath<TMCardData>(AssetDatabase.GUIDToAssetPath(guid));

                FieldInfo fieldInfo = cardData.GetType().GetField("_specialEffect", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo?.GetValue(cardData) is List<ITMCardSpecialEffect> specialEffects)
                {
                    foreach (ITMCardSpecialEffect cardSpecialEffect in specialEffects)
                    {
                        if (cardSpecialEffect is not HoldEffect holdCard || holdCard != this) continue;

                        Debug.LogWarning("보유 효과의 카드는 자기 자신을 트리거 카드로 지정할 수 없습니다");
                        _friendlyCard = null;
                    }
                }
            }
        }
    }
}
#endif
