using System;
using UnityEditor;

namespace AYellowpaper.SerializedCollections.Editor.Search
{
    public class EnumMatcher : Matcher
    {
        public override bool IsMatch(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Enum && SCEditorUtility.TryGetTypeFromProperty(property, out Type type))
            {
                foreach (string text in SCEnumUtility.GetEnumCache(type).GetNamesForValue(property.enumValueFlag))
                {
                    if (text.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }
    }
}