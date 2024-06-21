#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using OnwAttributeExtensions;
using UnityEngine;
using UnityEditor;

public sealed partial class TMCardData : ScriptableObject
{
    private void OnEnable()
    {
        var cards = AssetDatabase.FindAssets("t:TMCardData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<TMCardData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(card => card.Guid != Guid);

        StackID = cards.Count() + 1;

        GroupStackID = cards
            .GroupBy(card => card.CardGroup)
            .Where(group => group.Key == CardGroup)
            .Count() + 1;
    }

    [InspectorButton("Generate GUID")]
    private void generateNewGUID()
    {
        Guid = System.Guid.NewGuid().ToString();
    }

    [OnValueChangedByMethod(nameof(CardGroup))]
    private void setGroupStackID()
    {
        var cards = AssetDatabase.FindAssets("t:TMCardData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<TMCardData>(AssetDatabase.GUIDToAssetPath(guid)));

        foreach (var group in cards.GroupBy(card => card.CardGroup))
        {
            var groupArray = group.ToArray();

            for (int i = 0; i < groupArray.Length; i++)
            {
                groupArray[i].GroupStackID = i + 1;
            }
        }
    }
}
#endif