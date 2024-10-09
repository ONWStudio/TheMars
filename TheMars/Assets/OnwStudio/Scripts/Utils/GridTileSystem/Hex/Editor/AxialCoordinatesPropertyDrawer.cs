#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Editor;

namespace Onw.HexGrid.Editor
{
    [CustomPropertyDrawer(typeof(AxialCoordinates))]
    internal sealed class AxialCoordinatesPropertyDrawer : InitializablePropertyDrawer
    {
        private SerializedProperty _qSerializedProperty = null;
        private SerializedProperty _rSerializedProperty = null;
        
        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            _qSerializedProperty = property.FindPropertyRelative(EditorReflectionHelper.GetBackingFieldName("Q"));
            _rSerializedProperty = property.FindPropertyRelative(EditorReflectionHelper.GetBackingFieldName("R"));
        }
        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float labelSizeX = position.size.x * 0.15f;
            float propertySizeX = (position.size.x - labelSizeX) / 2;
            
            Rect labelRect = new(position.x, position.y, labelSizeX, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, "HexPoint : ");
            Rect qRect = new(position.x + labelSizeX, position.y, propertySizeX, EditorGUIUtility.singleLineHeight);
            _qSerializedProperty.intValue = EditorGUI.IntField(qRect, "Q ", _qSerializedProperty.intValue);
            Rect rRect = new(position.x + labelSizeX + propertySizeX, position.y, propertySizeX, EditorGUIUtility.singleLineHeight);
            _rSerializedProperty.intValue = EditorGUI.IntField(rRect, "R ", _rSerializedProperty.intValue);
        }
    }
}
#endif