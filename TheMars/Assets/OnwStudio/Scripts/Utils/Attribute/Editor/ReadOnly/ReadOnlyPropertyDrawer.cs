#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OnwAttributeExtensions;

namespace OnwAttributeExtensionsEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    internal sealed class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic || !property.isArray)
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
                SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
                int arraySize = arraySizeProp.intValue;

                for (int i = 0; i < arraySize; i++)
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
            if (property.propertyType != SerializedPropertyType.Generic || !property.isArray)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            if (property.isExpanded)
            {
                float height = EditorGUIUtility.singleLineHeight;

                SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
                int arraySize = arraySizeProp.intValue;

                for (int i = 0; i < arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.standardVerticalSpacing;
                }

                return height;
            }

            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif
