#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SocialsWidget))]
    public class SocialsWidgetEditor : Editor
    {
        private SocialsWidget swTarget;
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            swTarget = (SocialsWidget)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "Socials Widget Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            currentTab = HeatUIEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            SerializedProperty socials = serializedObject.FindProperty("socials");

            SerializedProperty itemPreset = serializedObject.FindProperty("itemPreset");
            SerializedProperty itemParent = serializedObject.FindProperty("itemParent");
            SerializedProperty buttonPreset = serializedObject.FindProperty("buttonPreset");
            SerializedProperty buttonParent = serializedObject.FindProperty("buttonParent");
            SerializedProperty background = serializedObject.FindProperty("background");

            SerializedProperty allowTransition = serializedObject.FindProperty("allowTransition");
            SerializedProperty useLocalization = serializedObject.FindProperty("useLocalization");
            SerializedProperty timer = serializedObject.FindProperty("timer");
            SerializedProperty tintSpeed = serializedObject.FindProperty("tintSpeed");
            SerializedProperty tintCurve = serializedObject.FindProperty("tintCurve");
            SerializedProperty updateMode = serializedObject.FindProperty("updateMode");

            switch (currentTab)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(socials, new GUIContent("Socials"), true);
                    EditorGUI.indentLevel = 0;
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(itemPreset, customSkin, "Item Preset");
                    HeatUIEditorHandler.DrawProperty(itemParent, customSkin, "Item Parent");
                    HeatUIEditorHandler.DrawProperty(buttonPreset, customSkin, "Button Preset");
                    HeatUIEditorHandler.DrawProperty(buttonParent, customSkin, "Button Parent");
                    HeatUIEditorHandler.DrawProperty(background, customSkin, "Background");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    allowTransition.boolValue = HeatUIEditorHandler.DrawToggle(allowTransition.boolValue, customSkin, "Allow Transition", "Pause or unpause the transition.");
                    useLocalization.boolValue = HeatUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    HeatUIEditorHandler.DrawProperty(timer, customSkin, "Timer");
                    HeatUIEditorHandler.DrawProperty(tintSpeed, customSkin, "Tint Speed");
                    HeatUIEditorHandler.DrawProperty(tintCurve, customSkin, "Tint Curve");
                    HeatUIEditorHandler.DrawProperty(updateMode, customSkin, "Update Mode");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif