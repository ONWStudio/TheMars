#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(CreditsManager))]
    public class CreditsManagerEditor : Editor
    {
        private CreditsManager cmTarget;
        private GUISkin customSkin;
        private int latestTabIndex;

        private void OnEnable()
        {
            cmTarget = (CreditsManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "Credits Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            latestTabIndex = HeatUIEditorHandler.DrawTabs(latestTabIndex, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                latestTabIndex = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                latestTabIndex = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                latestTabIndex = 2;

            GUILayout.EndHorizontal();

            SerializedProperty creditsPreset = serializedObject.FindProperty("creditsPreset");

            SerializedProperty canvasGroup = serializedObject.FindProperty("canvasGroup");
            SerializedProperty backgroundImage = serializedObject.FindProperty("backgroundImage");
            SerializedProperty creditsListParent = serializedObject.FindProperty("creditsListParent");
            SerializedProperty scrollHelper = serializedObject.FindProperty("scrollHelper");
            SerializedProperty creditsSectionPreset = serializedObject.FindProperty("creditsSectionPreset");
            SerializedProperty creditsMentionPreset = serializedObject.FindProperty("creditsMentionPreset");

            SerializedProperty closeAutomatically = serializedObject.FindProperty("closeAutomatically");
            SerializedProperty fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");
            SerializedProperty scrollDelay = serializedObject.FindProperty("scrollDelay");
            SerializedProperty scrollSpeed = serializedObject.FindProperty("scrollSpeed");
            SerializedProperty boostValue = serializedObject.FindProperty("boostValue");
            SerializedProperty boostHotkey = serializedObject.FindProperty("boostHotkey");

            SerializedProperty onOpen = serializedObject.FindProperty("onOpen");
            SerializedProperty onClose = serializedObject.FindProperty("onClose");
            SerializedProperty onCreditsEnd = serializedObject.FindProperty("onCreditsEnd");

            switch (latestTabIndex)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    HeatUIEditorHandler.DrawProperty(creditsPreset, customSkin, "Credits Preset");

                    HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(onOpen, new GUIContent("On Open"), true);
                    EditorGUILayout.PropertyField(onClose, new GUIContent("On Close"), true);
                    EditorGUILayout.PropertyField(onCreditsEnd, new GUIContent("On Credits End"), true);
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(canvasGroup, customSkin, "Canvas Group");
                    HeatUIEditorHandler.DrawProperty(backgroundImage, customSkin, "BG Image");
                    HeatUIEditorHandler.DrawProperty(creditsListParent, customSkin, "List Parent");
                    HeatUIEditorHandler.DrawProperty(scrollHelper, customSkin, "Scroll Helper");
                    HeatUIEditorHandler.DrawProperty(creditsSectionPreset, customSkin, "Section Preset");
                    HeatUIEditorHandler.DrawProperty(creditsMentionPreset, customSkin, "Mention Preset");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    closeAutomatically.boolValue = HeatUIEditorHandler.DrawToggle(closeAutomatically.boolValue, customSkin, "Close Automatically");
                    HeatUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");
                    HeatUIEditorHandler.DrawProperty(scrollDelay, customSkin, "Scroll Delay");
                    HeatUIEditorHandler.DrawProperty(scrollSpeed, customSkin, "Scroll Speed");
                    HeatUIEditorHandler.DrawProperty(boostValue, customSkin, "Boost Value");
                    EditorGUILayout.PropertyField(boostHotkey, new GUIContent("Boost Hotkey"), true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif