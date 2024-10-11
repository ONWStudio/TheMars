#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
using Onw.Editor;

namespace Onw.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    internal sealed class InfoBoxDrawer : PropertyDrawer
    {
        private readonly float _helpBoxHeight = EditorGUIUtility.singleLineHeight * 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InfoBoxAttribute infoBoxAttribute = (attribute as InfoBoxAttribute)!;
            
            MessageType messageType = infoBoxAttribute.InfoType switch
            {
                INFO_TYPE.INFO => MessageType.Info,
                INFO_TYPE.WARNING => MessageType.Warning,
                INFO_TYPE.ERROR => MessageType.Error,
                _ => MessageType.None,
            };
            
            Rect helpBoxRect = new(
                position.x,
                position.y,
                position.width,
                _helpBoxHeight);

            Rect propertyRect = new(
                position.x,
                position.y + _helpBoxHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(property, label, true));

            EditorGUI.HelpBox(helpBoxRect, infoBoxAttribute.Message, messageType);
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _helpBoxHeight + EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
#endif
