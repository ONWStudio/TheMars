﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi.Editor
{
    [CustomEditor(typeof(BetterContentSizeFitter)), CanEditMultipleObjects]
    public class BetterContentSizeFitterEditor : UnityEditor.Editor
    {
        private SerializedProperty settingsFallback, settingsList;
        private SerializedProperty minWidthFallback, minWidthList;
        private SerializedProperty minHeightFallback, minHeightList;
        private SerializedProperty maxWidthFallback, maxWidthList;
        private SerializedProperty maxHeightFallback, maxHeightList;
        private SerializedProperty paddingFallback, paddingList;
        private SerializedProperty source;
        private SerializedProperty treatAsLayoutElement;

        private bool showMinHeight, showMinWidth;
        private bool showMaxHeight, showMaxWidth;
        private bool experimentalExpanded;

        private void OnEnable()
        {
            this.settingsFallback = serializedObject.FindProperty("settingsFallback");
            this.settingsList = serializedObject.FindProperty("customSettings");

            minWidthFallback = serializedObject.FindProperty("minWidthSizerFallback");
            minWidthList = serializedObject.FindProperty("minWidthSizers");
            minHeightFallback = serializedObject.FindProperty("minHeightSizerFallback");
            minHeightList = serializedObject.FindProperty("minHeightSizers");

            maxWidthFallback = serializedObject.FindProperty("maxWidthSizerFallback");
            maxWidthList = serializedObject.FindProperty("maxWidthSizers");
            maxHeightFallback = serializedObject.FindProperty("maxHeightSizerFallback");
            maxHeightList = serializedObject.FindProperty("maxHeightSizers");

            paddingFallback = serializedObject.FindProperty("paddingFallback");
            paddingList = serializedObject.FindProperty("paddingSizers");

            source = serializedObject.FindProperty("source");
            treatAsLayoutElement = serializedObject.FindProperty("treatAsLayoutElement");

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(source);
            EditorGUILayout.PropertyField(treatAsLayoutElement);

            ScreenConfigConnectionHelper.DrawGui("Settings", settingsList, ref settingsFallback, DrawSettings);

            if (showMinWidth)
            {
                ScreenConfigConnectionHelper.DrawSizerGui("Min Width", minWidthList, ref minWidthFallback);
            }

            if (showMaxWidth)
            {
                ScreenConfigConnectionHelper.DrawSizerGui("Max Width", maxWidthList, ref maxWidthFallback);
            }

            if (showMinHeight)
            {
                ScreenConfigConnectionHelper.DrawSizerGui("Min Height", minHeightList, ref minHeightFallback);
            }

            if (showMaxHeight)
            {
                ScreenConfigConnectionHelper.DrawSizerGui("Max Height", maxHeightList, ref maxHeightFallback);
            }

            ScreenConfigConnectionHelper.DrawSizerGui("Padding", paddingList, ref paddingFallback);
        }

        private void DrawSettings(string configName, SerializedProperty settings)
        {
            SerializedProperty horizontalFit = settings.FindPropertyRelative("HorizontalFit");
            SerializedProperty verticalFit = settings.FindPropertyRelative("VerticalFit");
            SerializedProperty hasMinWidth = settings.FindPropertyRelative("HasMinWidth");
            SerializedProperty hasMinHeight = settings.FindPropertyRelative("HasMinHeight");
            SerializedProperty hasMaxWidth = settings.FindPropertyRelative("HasMaxWidth");
            SerializedProperty hasMaxHeight = settings.FindPropertyRelative("HasMaxHeight");

            EditorGUILayout.PropertyField(horizontalFit);
            if (horizontalFit.intValue != 0)
            {
                EditorGUILayout.PropertyField(hasMinWidth);
                EditorGUILayout.PropertyField(hasMaxWidth);
                EditorGUILayout.Space();
            }

            EditorGUILayout.PropertyField(verticalFit);
            if (verticalFit.intValue != 0)
            {
                EditorGUILayout.PropertyField(hasMinHeight);
                EditorGUILayout.PropertyField(hasMaxHeight);
                EditorGUILayout.Space();
            }
            
            showMinWidth = hasMinWidth.boolValue;
            showMinHeight = hasMinHeight.boolValue;
            showMaxWidth = hasMaxWidth.boolValue;
            showMaxHeight = hasMaxHeight.boolValue;

            EditorGUI.indentLevel++;
            experimentalExpanded = EditorGUILayout.Foldout(experimentalExpanded, "Experimental");

            if (experimentalExpanded)
            {
                SerializedProperty isAnimated = settings.FindPropertyRelative("IsAnimated");
                SerializedProperty animationTime = settings.FindPropertyRelative("AnimationTime");

                EditorGUILayout.PropertyField(isAnimated);
                if (isAnimated.boolValue)
                {
                    EditorGUILayout.PropertyField(animationTime);
                }
            }

            EditorGUI.indentLevel--;

        }

        [MenuItem("CONTEXT/ContentSizeFitter/♠ Make Better")]
        public static void MakeBetter(MenuCommand command)
        {
            ContentSizeFitter fitter = command.context as ContentSizeFitter;
            ContentSizeFitter.FitMode h = fitter.horizontalFit;
            ContentSizeFitter.FitMode v = fitter.verticalFit;

            BetterContentSizeFitter newFitter = Betterizer.MakeBetter<ContentSizeFitter, BetterContentSizeFitter>(fitter) as BetterContentSizeFitter;
            if (newFitter != null)
            {
                newFitter.CurrentSettings.HorizontalFit = h;
                newFitter.CurrentSettings.VerticalFit = v;
            }
        }
    }
}
