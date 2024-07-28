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
        private void onChangedFriendlyCard()
        {
            Debug.Log("changed friendlyCard");

            foreach (string guid in AssetDatabase.FindAssets($"t:{typeof(TMCardData).Name}"))
            {
                TMCardData cardData = AssetDatabase
                    .LoadAssetAtPath<TMCardData>(AssetDatabase.GUIDToAssetPath(guid));

                FieldInfo fieldInfo = cardData.GetType().GetField("_effectCreators", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo?.GetValue(cardData) is List<ITMEffectCreator> specialEffects)
                {
                    foreach (ITMEffectCreator cardSpecialEffect in specialEffects)
                    {
                        if (cardSpecialEffect is not HoldEffectCreator holdCard || holdCard != this) continue;

                        Debug.LogWarning("���� ȿ���� ī��� �ڱ� �ڽ��� Ʈ���� ī��� ������ �� �����ϴ�");
                        FriendlyCard = null;
                    }
                }
            }
        }
    }
}
#endif
