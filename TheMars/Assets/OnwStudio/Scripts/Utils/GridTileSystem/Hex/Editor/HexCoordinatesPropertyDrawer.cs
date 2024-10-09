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
        private SerializedProperty _qSerializedProperty = null;
        private SerializedProperty _rSerializedProperty = null;
        private SerializedProperty _sSerializedProperty = null;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            _qSerializedProperty = property.FindPropertyRelative(EditorReflectionHelper.GetBackingFieldName("Q"));
            _rSerializedProperty = property.FindPropertyRelative(EditorReflectionHelper.GetBackingFieldName("R"));
            _sSerializedProperty = property.FindPropertyRelative(EditorReflectionHelper.GetBackingFieldName("S"));
        }
        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float labelSizeX = position.size.x * 0.15f;
            
            Rect labelRect = new(position.x, position.y, labelSizeX, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, "HexPoint : ");
            Rect qRect = new(position.x + labelSizeX, position.y, 20, EditorGUIUtility.singleLineHeight);
            _qSerializedProperty.intValue = EditorGUI.IntField(qRect, "Q ", _qSerializedProperty.intValue);
            Rect rRect = new(position.x + labelSizeX + 20, position.y, 20, EditorGUIUtility.singleLineHeight);
            _rSerializedProperty.intValue = EditorGUI.IntField(rRect, "R ", _rSerializedProperty.intValue);
            Rect sRect = new(position.x + labelSizeX + 20 * 2, position.y, 20, EditorGUIUtility.singleLineHeight);
            _sSerializedProperty.intValue = EditorGUI.IntField(sRect, "S ", _sSerializedProperty.intValue);
        }
    }
}
#endif
