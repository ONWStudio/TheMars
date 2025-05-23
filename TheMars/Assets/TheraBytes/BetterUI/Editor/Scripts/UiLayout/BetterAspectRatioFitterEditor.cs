﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine.UI;

namespace TheraBytes.BetterUi.Editor
{
    [CustomEditor(typeof(BetterAspectRatioFitter)), CanEditMultipleObjects]
    public class BetterAspectRatioFitterEditor : UnityEditor.Editor
    {
        private SerializedProperty settingsFallback, settingsList;

        private void OnEnable()
        {
            this.settingsFallback = serializedObject.FindProperty("settingsFallback");
            this.settingsList = serializedObject.FindProperty("customSettings");
        }
        
        public override void OnInspectorGUI()
        {
            ScreenConfigConnectionHelper.DrawGui("Settings", settingsList, ref settingsFallback, DrawSettings);
        }

        private void DrawSettings(string configName, SerializedProperty settings)
        {
            SerializedProperty mode = settings.FindPropertyRelative("AspectMode");
            SerializedProperty ratio = settings.FindPropertyRelative("AspectRatio");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(mode);
            EditorGUILayout.PropertyField(ratio);

            EditorGUILayout.EndVertical();
        }

        [MenuItem("CONTEXT/AspectRatioFitter/♠ Make Better")]
        public static void MakeBetter(MenuCommand command)
        {
            AspectRatioFitter fitter = command.context as AspectRatioFitter;
            AspectRatioFitter.AspectMode mode = fitter.aspectMode;
            float ratio = fitter.aspectRatio;

            BetterAspectRatioFitter newFitter = Betterizer.MakeBetter<AspectRatioFitter, BetterAspectRatioFitter>(fitter) as BetterAspectRatioFitter;
            if(newFitter != null)
            {
                newFitter.CurrentSettings.AspectMode = mode;
                newFitter.CurrentSettings.AspectRatio = ratio;
            }
        }
    }
}
