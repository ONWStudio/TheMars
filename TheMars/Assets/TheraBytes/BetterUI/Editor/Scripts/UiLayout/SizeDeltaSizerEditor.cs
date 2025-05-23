﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{
    [CustomEditor(typeof(SizeDeltaSizer)), CanEditMultipleObjects]
    public class SizeDeltaSizerEditor : UnityEditor.Editor
    {
        private SerializedProperty settingsFallback, settingsList;
        private SerializedProperty deltaSizerFallback, deltaSizerCollection;

        private void OnEnable()
        {
            this.settingsFallback = serializedObject.FindProperty("settingsFallback");
            this.settingsList = serializedObject.FindProperty("customSettings");

            deltaSizerFallback = serializedObject.FindProperty("deltaSizerFallback");
            deltaSizerCollection = serializedObject.FindProperty("customDeltaSizers");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Size Delta Sizer is obsolete. Use Better Offsetter instead.", MessageType.Warning);
            if(GUILayout.Button("♠ Convert to Better Offsetter", GUILayout.Height(30)))
            {
                BetterOffsetterEditor.ConvertToBetterOffsetter(target as SizeDeltaSizer);
                return;
            }

            ScreenConfigConnectionHelper.DrawGui("Settings", settingsList, ref settingsFallback, DrawSettings);
            ScreenConfigConnectionHelper.DrawSizerGui("Size Delta Settings", deltaSizerCollection, ref deltaSizerFallback);
        }

        private void DrawSettings(string configName, SerializedProperty settings)
        {
            SerializedProperty w = settings.FindPropertyRelative("applyWidth");
            SerializedProperty h = settings.FindPropertyRelative("applyHeight");

            EditorGUILayout.PropertyField(w);
            EditorGUILayout.PropertyField(h);
        }
    }
}
