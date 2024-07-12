#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;

namespace Onw.Editor.Attribute
{
    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    internal sealed class InfoBoxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InfoBoxAttribute infoBoxAttribute = attribute as InfoBoxAttribute;

            float helpBoxHeight = EditorGUIUtility.singleLineHeight * 2;

            Rect helpBoxRect = new(
                position.x,
                position.y,
                position.width,
                helpBoxHeight);

            Rect propertyRect = new(
                position.x,
                position.y + helpBoxHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(property, label, true));

            MessageType messageType = infoBoxAttribute.InfoType switch
            {
                INFO_TYPE.INFO => MessageType.Info,
                INFO_TYPE.WARNING => MessageType.Warning,
                INFO_TYPE.ERROR => MessageType.Error,
                _ => MessageType.None,
            };

            EditorGUI.HelpBox(helpBoxRect, infoBoxAttribute.Message, messageType);
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float helpBoxHeight = EditorGUIUtility.singleLineHeight * 2;
            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            return helpBoxHeight + propertyHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
#endif
