#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Editor;

namespace Onw.HexGrid.Editor
{
    [CustomPropertyDrawer(typeof(HexCoordinates))]
    internal sealed class HexCoordinatesPropertyDrawer : InitializablePropertyDrawer
    {
        private SerializedProperty _qProp = null;
        private SerializedProperty _rProp = null;
        private SerializedProperty _sProp = null;
        private GUIStyle _labelStyle = null;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            _qProp = property.FindPropertyRelative("_q");
            _rProp = property.FindPropertyRelative("_r");
            _sProp = property.FindPropertyRelative("_s");
            _labelStyle = new(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            };
        }
        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using EditorGUI.PropertyScope propScope = new(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            float labelWidth = EditorGUIUtility.singleLineHeight;  // 각 라벨의 너비
            float fieldWidth = (position.width - labelWidth * 3) / 3f;

            // Q 필드
            Rect qRect = new(position.x, position.y, labelWidth, position.height);
            EditorGUI.LabelField(qRect, "Q", _labelStyle);
            Rect qFieldRect = new(position.x + labelWidth, position.y, fieldWidth, position.height);
            _qProp.intValue = EditorGUI.IntField(qFieldRect, _qProp.intValue);

            // R 필드
            Rect rRect = new(position.x + labelWidth + fieldWidth, position.y, labelWidth, position.height);
            EditorGUI.LabelField(rRect, "R", _labelStyle);
            Rect rFieldRect = new(position.x + (labelWidth + fieldWidth) + labelWidth, position.y, fieldWidth, position.height);
            _rProp.intValue = EditorGUI.IntField(rFieldRect, _rProp.intValue);

            // S 필드
            Rect sRect = new(position.x + (labelWidth + fieldWidth) * 2, position.y, labelWidth, position.height);
            EditorGUI.LabelField(sRect, "S", _labelStyle);
            Rect sFieldRect = new(position.x + (labelWidth + fieldWidth) * 2 + labelWidth, position.y, fieldWidth, position.height);
            _sProp.intValue = EditorGUI.IntField(sFieldRect, _sProp.intValue);
        }
    }
}
#endif