#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NewsSlider))]
    public class NewsSliderEditor : Editor
    {
        private NewsSlider nsTarget;
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            nsTarget = (NewsSlider)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "News Slider Top Header");

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

            SerializedProperty items = serializedObject.FindProperty("items");

            SerializedProperty itemPreset = serializedObject.FindProperty("itemPreset");
            SerializedProperty itemParent = serializedObject.FindProperty("itemParent");
            SerializedProperty timerPreset = serializedObject.FindProperty("timerPreset");
            SerializedProperty timerParent = serializedObject.FindProperty("timerParent");

            SerializedProperty allowUpdate = serializedObject.FindProperty("allowUpdate");
            SerializedProperty useLocalization = serializedObject.FindProperty("useLocalization");
            SerializedProperty sliderTimer = serializedObject.FindProperty("sliderTimer");
            SerializedProperty updateMode = serializedObject.FindProperty("updateMode");

            switch (currentTab)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(items, new GUIContent("Slider Items"), true);
                    EditorGUI.indentLevel = 0;
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(itemPreset, customSkin, "Item Preset");
                    HeatUIEditorHandler.DrawProperty(itemParent, customSkin, "Item Parent");
                    HeatUIEditorHandler.DrawProperty(timerPreset, customSkin, "Timer Preset");
                    HeatUIEditorHandler.DrawProperty(timerParent, customSkin, "Timer Parent");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    allowUpdate.boolValue = HeatUIEditorHandler.DrawToggle(allowUpdate.boolValue, customSkin, "Allow Update", "Pause or unpause the slider.");
                    useLocalization.boolValue = HeatUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    HeatUIEditorHandler.DrawProperty(sliderTimer, customSkin, "Slider Timer");
                    HeatUIEditorHandler.DrawProperty(updateMode, customSkin, "Update Mode");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif