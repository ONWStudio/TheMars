#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;

namespace Onw.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(DisplayAsAttribute))]
    internal sealed class DisplayAsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisplayAsAttribute displayAs = attribute as DisplayAsAttribute;

            label.text = displayAs.DisplayName;

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
