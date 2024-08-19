using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi.Editor
{
    [CustomEditor(typeof(ValueDragger)), CanEditMultipleObjects]
    public class ValueDraggerEditor : UnityEditor.Editor
    {
        private SerializedProperty
            fallbackDragSettings, customDragSettings,
            fallbackValueSettings, customValueSettings,
            fallbackDragDistance, customDragDistance,
            value, onValueChanged;

        // as ValueDragger derives from BetterSelectable, we need to use this.
        private BetterElementHelper<Selectable, BetterSelectable> helper =
            new BetterElementHelper<Selectable, BetterSelectable>();

        private void OnEnable()
        {
            fallbackDragSettings = serializedObject.FindProperty("fallbackDragSettings");
            customDragSettings = serializedObject.FindProperty("customDragSettings");
            fallbackValueSettings = serializedObject.FindProperty("fallbackValueSettings");
            customValueSettings = serializedObject.FindProperty("customValueSettings");
            fallbackDragDistance = serializedObject.FindProperty("fallbackDragDistance");
            customDragDistance = serializedObject.FindProperty("customDragDistance");
            value = serializedObject.FindProperty("value");
            onValueChanged = serializedObject.FindProperty("onValueChanged");

        }

        public override void OnInspectorGUI()
        {
            ScreenConfigConnectionHelper.DrawGui("Drag Settings",
                customDragSettings, ref fallbackDragSettings, DrawDragSettings);

            ScreenConfigConnectionHelper.DrawGui("Value Settings",
                customValueSettings, ref fallbackValueSettings, DrawValueSettings);

            ScreenConfigConnectionHelper.DrawSizerGui("Drag Distance per Integer Step",
                customDragDistance, ref fallbackDragDistance);

            EditorGUILayout.PropertyField(value);
            EditorGUILayout.PropertyField(onValueChanged);

            helper.DrawGui(serializedObject);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDragSettings(string configName, SerializedProperty prop)
        {
            SerializedProperty direction = prop.FindPropertyRelative("Direction");
            SerializedProperty invert = prop.FindPropertyRelative("Invert");

            EditorGUILayout.PropertyField(direction);
            EditorGUILayout.PropertyField(invert);
        }

        private void DrawValueSettings(string configName, SerializedProperty prop)
        {
            SerializedProperty hasMinValue = prop.FindPropertyRelative("HasMinValue");
            SerializedProperty minValue = prop.FindPropertyRelative("MinValue");

            SerializedProperty hasMaxValue = prop.FindPropertyRelative("HasMaxValue");
            SerializedProperty maxValue = prop.FindPropertyRelative("MaxValue");

            SerializedProperty wholeNumbers = prop.FindPropertyRelative("WholeNumbers");

            DrawCheckboxField("Min Value", hasMinValue, minValue);
            DrawCheckboxField("Max Value", hasMaxValue, maxValue);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(wholeNumbers);
        }

        private static void DrawCheckboxField(string label, SerializedProperty checkValue, SerializedProperty fieldValue)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            Rect checkRect = checkValue.boolValue
                ? new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, rect.height)
                : rect;

            checkValue.boolValue = EditorGUI.ToggleLeft(checkRect, label, checkValue.boolValue);
            if (checkValue.boolValue)
            {
                Rect fieldRect = new Rect(checkRect.xMax, rect.y, rect.xMax - checkRect.xMax, rect.height);
                EditorGUI.PropertyField(fieldRect, fieldValue, GUIContent.none);
            }

        }
    }
}
