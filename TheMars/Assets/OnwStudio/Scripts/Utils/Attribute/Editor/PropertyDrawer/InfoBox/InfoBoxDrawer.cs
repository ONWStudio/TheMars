#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;

namespace Onw.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    internal sealed class InfoBoxDrawer : InitializablePropertyDrawer
    {
        private readonly float _helpBoxHeight = EditorGUIUtility.singleLineHeight * 2f;
        private InfoBoxAttribute _attribute;
        private MessageType _messageType;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            _attribute = attribute as InfoBoxAttribute;
            _messageType = _attribute.InfoType switch
            {
                INFO_TYPE.INFO => MessageType.Info,
                INFO_TYPE.WARNING => MessageType.Warning,
                INFO_TYPE.ERROR => MessageType.Error,
                _ => MessageType.None,
            };
        }

        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
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

            EditorGUI.HelpBox(helpBoxRect, _attribute.Message, _messageType);
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _helpBoxHeight + EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
#endif
