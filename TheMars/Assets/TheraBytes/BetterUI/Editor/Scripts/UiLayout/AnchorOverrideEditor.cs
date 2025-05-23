﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{

    [CustomEditor(typeof(AnchorOverride)), CanEditMultipleObjects]
    public class AnchorOverrideEditor : UnityEditor.Editor
    {
        private static readonly GUIContent IsAnimatedContent = new GUIContent("Animate",
            "If enabled the anchors are not applied instantly but with an animation.");

        private static readonly GUIContent AccelerationContent = new GUIContent("Acceleration",
            "Defines how fast the movement reaches maximum speed.");

        private static readonly GUIContent MaxMoveSpeedContent = new GUIContent("Max Move Speed",
            "The maximum velocity when moving.");

        private static readonly GUIContent SnapThresholdContent = new GUIContent("Snap Threshold",
            "The distance of the farest anchor / axis, at which the animation stops and the target anchors are applied.");

        private static readonly string[] HorizontalOptions = { "Center", "Pivot", "Left", "Right" };
        private static readonly string[] VerticalOptions = { "Center", "Pivot", "Bottom", "Top" };

        private SerializedProperty anchorsFallback, anchorsConfigs,
            mode, isAnimated, acceleration, maxMoveSpeed, snapThreshold;

        private Dictionary<string, UnityEditorInternal.ReorderableList> lists = new Dictionary<string, UnityEditorInternal.ReorderableList>();

        private void OnEnable()
        {
            //AnchorOverride ao = target as AnchorOverride;

            anchorsFallback = serializedObject.FindProperty("anchorsFallback");
            anchorsConfigs = serializedObject.FindProperty("anchorsConfigs");

            mode = serializedObject.FindProperty("mode");
            isAnimated = serializedObject.FindProperty("isAnimated");
            acceleration = serializedObject.FindProperty("acceleration");
            maxMoveSpeed = serializedObject.FindProperty("maxMoveSpeed");
            snapThreshold = serializedObject.FindProperty("snapThreshold");

            IntroduceList(ResolutionMonitor.Instance.FallbackName + " (Fallback)", anchorsFallback.FindPropertyRelative("elements"));

            SerializedProperty items = anchorsConfigs.FindPropertyRelative("items");
            for (int i = 0; i < items.arraySize; i++)
            {
                SerializedProperty prop = items.GetArrayElementAtIndex(i);
                SerializedProperty elements = prop.FindPropertyRelative("elements");
                SerializedProperty configNameProp = prop.FindPropertyRelative("screenConfigName");
                string configName = configNameProp.stringValue;

                IntroduceList(configName, elements);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            DrawModeSelection();
            DrawAnimationSettings();

            EditorGUILayout.Space();

            ScreenConfigConnectionHelper.DrawGui("Anchors", anchorsConfigs, ref anchorsFallback, DrawAnchorSettings);
        }

        private void DrawModeSelection()
        {
            if (isAnimated.boolValue)
            {
                EditorGUILayout.PropertyField(mode);
            }
            else
            {
                string[] options = { "Auto Update", /* enum index 1 is skipped, */ "Manual Update" };

                int prevIndex = (mode.enumValueIndex == 2) ? 1 : 0;
                int newIndex = EditorGUILayout.Popup("Mode", prevIndex, options);
                if (newIndex != prevIndex)
                {
                    mode.enumValueIndex = (newIndex == 1) ? 2 : 0;
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void DrawAnimationSettings()
        {
            EditorGUILayout.PropertyField(isAnimated, IsAnimatedContent);
            if (isAnimated.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(acceleration, AccelerationContent);
                EditorGUILayout.PropertyField(maxMoveSpeed, MaxMoveSpeedContent);
                EditorGUILayout.PropertyField(snapThreshold, SnapThresholdContent);
                EditorGUI.indentLevel--;
            }
        }

        private void DrawAnchorSettings(string configName, SerializedProperty prop)
        {
            SerializedProperty elements = prop.FindPropertyRelative("elements");
            IntroduceList(configName, elements);


            lists[configName].DoLayoutList();
        }


        private void IntroduceList(string configName, SerializedProperty elements)
        {
            if (lists.ContainsKey(configName))
                return;

            const float SPACE = 5;

            ReorderableList list = new UnityEditorInternal.ReorderableList(serializedObject, elements);
            list.elementHeight = 5 * EditorGUIUtility.singleLineHeight + 3 * SPACE;

            list.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                Rect r = new Rect(rect.x, rect.y + SPACE, rect.width, EditorGUIUtility.singleLineHeight);
                SerializedProperty prop = elements.GetArrayElementAtIndex(index);
                SerializedProperty reference = prop.FindPropertyRelative("reference");
                EditorGUI.PropertyField(r, reference);

                r.y += EditorGUIUtility.singleLineHeight + SPACE;
                SerializedProperty minX = prop.FindPropertyRelative("minX");
                DrawAnchorEdgeSetting("Min X", minX, r, AnchorOverride.AnchorReference.ReferenceLocation.LowerLeft, HorizontalOptions);

                r.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty maxX = prop.FindPropertyRelative("maxX");
                DrawAnchorEdgeSetting("Max X", maxX, r, AnchorOverride.AnchorReference.ReferenceLocation.UpperRight, HorizontalOptions);

                r.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty minY = prop.FindPropertyRelative("minY");
                DrawAnchorEdgeSetting("Min Y", minY, r, AnchorOverride.AnchorReference.ReferenceLocation.LowerLeft, VerticalOptions);

                r.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty maxY = prop.FindPropertyRelative("maxY");
                DrawAnchorEdgeSetting("Max Y", maxY, r, AnchorOverride.AnchorReference.ReferenceLocation.UpperRight, VerticalOptions);
            };

            list.drawHeaderCallback += (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Anchor Overrides");
            };

            lists.Add(configName, list);
        }

        private void DrawAnchorEdgeSetting(string label, SerializedProperty edge, Rect r, AnchorOverride.AnchorReference.ReferenceLocation defaultLocation, string[] displayOptions)
        {
            bool somethingChanged = false;
            Rect checkRect = new Rect(r.x, r.y, 60, r.height);
            AnchorOverride.AnchorReference.ReferenceLocation location = (AnchorOverride.AnchorReference.ReferenceLocation)edge.enumValueIndex;
            bool prevChecked = location != AnchorOverride.AnchorReference.ReferenceLocation.Disabled;

            if (EditorGUI.ToggleLeft(checkRect, label, location != AnchorOverride.AnchorReference.ReferenceLocation.Disabled))
            {
                if (!prevChecked)
                {
                    edge.enumValueIndex = (int)defaultLocation;
                    somethingChanged = true;
                }

                Rect popupRect = new Rect(r.x + checkRect.width, r.y, r.width - checkRect.width, r.height);
                int prevIndex = edge.enumValueIndex - 1;
                int index = EditorGUI.Popup(popupRect, edge.enumValueIndex - 1, displayOptions);
                if (index != prevIndex)
                {
                    edge.enumValueIndex = index + 1;
                    somethingChanged = true;
                }
            }
            else if (prevChecked)
            {
                edge.enumValueIndex = (int)AnchorOverride.AnchorReference.ReferenceLocation.Disabled;
                somethingChanged = true;
            }

            if (somethingChanged)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
