#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
using Onw.Editor;

namespace Onw.Attribute.Editor
{
    using GUI = UnityEngine.GUI;

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    internal sealed class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        private readonly Dictionary<int, int?> _cachedArraySizeDictionary = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int hash = property.GetHashCode();
            
            if (!_cachedArraySizeDictionary.TryGetValue(hash, out int? arraySize))
            {
                arraySize = property.propertyType != SerializedPropertyType.Generic || !property.isArray ? null : property.FindPropertyRelative("Array.size").intValue;

                _cachedArraySizeDictionary.Add(hash, arraySize);
            }
            
            if (arraySize is null)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel++;

            if (property.isExpanded)
            {
                for (int i = 0; i < (int)arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(position, element, GUIContent.none, true);
                    position.y += EditorGUI.GetPropertyHeight(element, true) +
                        EditorGUIUtility.standardVerticalSpacing;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!_cachedArraySizeDictionary.TryGetValue(property.GetHashCode(), out int? arraySize) || arraySize is null)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                for (int i = 0; i < (int)arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }
    }
}
#endif
