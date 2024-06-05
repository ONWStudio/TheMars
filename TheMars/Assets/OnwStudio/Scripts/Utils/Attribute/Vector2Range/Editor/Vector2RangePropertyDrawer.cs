#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Vector2RangeSpace;
using static EditorTool.EditorTool;

namespace Vector2RangeEditor
{
    [CustomPropertyDrawer(typeof(Vector2RangeAttribute), true)]
    public sealed class Vector2RangePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Vector2RangeAttribute range = attribute as Vector2RangeAttribute;

            EditorGUILayout.LabelField(property.displayName);
            Vector2 vector = property.vector2Value;
            ActionEditorVertical(() =>
            {
                vector.x = EditorGUILayout.Slider("X", vector.x, range.Min, range.Max);
                vector.y = EditorGUILayout.Slider("Y", vector.y, range.Min, range.Max);
            }, GUI.skin.box);

            property.vector2Value = vector;
        }
    }
}
#endif