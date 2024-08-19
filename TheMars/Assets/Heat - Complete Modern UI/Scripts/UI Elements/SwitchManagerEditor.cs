#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(SwitchManager))]
    public class SwitchManagerEditor : Editor
    {
        private GUISkin customSkin;
        private SwitchManager switchTarget;
        private int currentTab;

        private void OnEnable()
        {
            switchTarget = (SwitchManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "Switch Top Header");

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

            SerializedProperty onValueChanged = serializedObject.FindProperty("onValueChanged");
            SerializedProperty onEvents = serializedObject.FindProperty("onEvents");
            SerializedProperty offEvents = serializedObject.FindProperty("offEvents");

            SerializedProperty switchAnimator = serializedObject.FindProperty("switchAnimator");
            SerializedProperty highlightCG = serializedObject.FindProperty("highlightCG");
            SerializedProperty audioManager = serializedObject.FindProperty("audioManager");

            SerializedProperty fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");
            SerializedProperty isOn = serializedObject.FindProperty("isOn");
            SerializedProperty isInteractable = serializedObject.FindProperty("isInteractable");
            SerializedProperty invokeOnEnable = serializedObject.FindProperty("invokeOnEnable");
            SerializedProperty useSounds = serializedObject.FindProperty("useSounds");
            SerializedProperty useUINavigation = serializedObject.FindProperty("useUINavigation");
            SerializedProperty saveValue = serializedObject.FindProperty("saveValue");
            SerializedProperty saveKey = serializedObject.FindProperty("saveKey");

            switch (currentTab)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    isOn.boolValue = HeatUIEditorHandler.DrawToggle(isOn.boolValue, customSkin, "Is On");
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    saveValue.boolValue = HeatUIEditorHandler.DrawTogglePlain(saveValue.boolValue, customSkin, "Save Value");
                    GUILayout.Space(3);

                    if (saveValue.boolValue == true)
                    {
                        HeatUIEditorHandler.DrawPropertyPlainCW(saveKey, customSkin, "Save Key:", 70);
                        EditorGUILayout.HelpBox("Each switch should has its own unique key.", MessageType.Info);
                    }

                    GUILayout.EndVertical();

                    HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(onValueChanged, new GUIContent("On Value Changed"), true);
                    EditorGUILayout.PropertyField(onEvents, new GUIContent("On Events"), true);
                    EditorGUILayout.PropertyField(offEvents, new GUIContent("Off Events"), true);
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(switchAnimator, customSkin, "Switch hAnimator");
                    HeatUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    isOn.boolValue = HeatUIEditorHandler.DrawToggle(isOn.boolValue, customSkin, "Is On");
                    isInteractable.boolValue = HeatUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    invokeOnEnable.boolValue = HeatUIEditorHandler.DrawToggle(invokeOnEnable.boolValue, customSkin, "Invoke On Enable", "Process events on enable.");
                    useSounds.boolValue = HeatUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Switch Sounds");
                    useUINavigation.boolValue = HeatUIEditorHandler.DrawToggle(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    saveValue.boolValue = HeatUIEditorHandler.DrawTogglePlain(saveValue.boolValue, customSkin, "Save Value");
                    GUILayout.Space(3);

                    if (saveValue.boolValue == true)
                    {
                        HeatUIEditorHandler.DrawPropertyPlainCW(saveKey, customSkin, "Save Key:", 70);
                        EditorGUILayout.HelpBox("Each switch should has its own unique key.", MessageType.Info);
                    }

                    GUILayout.EndVertical();

                    HeatUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif