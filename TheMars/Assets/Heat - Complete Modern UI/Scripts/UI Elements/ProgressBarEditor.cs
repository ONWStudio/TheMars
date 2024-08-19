#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(ProgressBar))]
    public class ProgressBarEditor : Editor
    {
        private GUISkin customSkin;
        private ProgressBar pbTarget;
        private int currentTab;

        private void OnEnable()
        {
            pbTarget = (ProgressBar)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "Progress Bar Top Header");

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

            SerializedProperty icon = serializedObject.FindProperty("icon");
            SerializedProperty currentValue = serializedObject.FindProperty("currentValue");
            SerializedProperty minValue = serializedObject.FindProperty("minValue");
            SerializedProperty maxValue = serializedObject.FindProperty("maxValue");
            SerializedProperty minValueLimit = serializedObject.FindProperty("minValueLimit");
            SerializedProperty maxValueLimit = serializedObject.FindProperty("maxValueLimit");

            SerializedProperty barImage = serializedObject.FindProperty("barImage");
            SerializedProperty iconObject = serializedObject.FindProperty("iconObject");
            SerializedProperty altIconObject = serializedObject.FindProperty("altIconObject");
            SerializedProperty textObject = serializedObject.FindProperty("textObject");
            SerializedProperty altTextObject = serializedObject.FindProperty("altTextObject");

            SerializedProperty addPrefix = serializedObject.FindProperty("addPrefix");
            SerializedProperty addSuffix = serializedObject.FindProperty("addSuffix");
            SerializedProperty prefix = serializedObject.FindProperty("prefix");
            SerializedProperty suffix = serializedObject.FindProperty("suffix");
            SerializedProperty decimals = serializedObject.FindProperty("decimals");
            SerializedProperty barDirection = serializedObject.FindProperty("barDirection");

            SerializedProperty onValueChanged = serializedObject.FindProperty("onValueChanged");

            switch (currentTab)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    if (pbTarget.barImage != null) { HeatUIEditorHandler.DrawPropertyCW(icon, customSkin, "Bar Icon", 99); }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(new GUIContent("Current Percent"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                    currentValue.floatValue = EditorGUILayout.Slider(pbTarget.currentValue, minValue.floatValue, maxValue.floatValue);
                    GUILayout.EndHorizontal();

                    pbTarget.UpdateUI();

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(new GUIContent("Min / Max Value"), customSkin.FindStyle("Text"), GUILayout.Width(110));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(2);

                    minValue.floatValue = EditorGUILayout.Slider(minValue.floatValue, minValueLimit.floatValue, maxValue.floatValue - 1);
                    maxValue.floatValue = EditorGUILayout.Slider(maxValue.floatValue, minValue.floatValue + 1, maxValueLimit.floatValue);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox("You can increase the min/max value limit by changing 'Value Limit' in the settings tab.", MessageType.Info);
                    GUILayout.EndVertical();

                    HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(onValueChanged, new GUIContent("On Value Changed"));
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(barImage, customSkin, "Bar Image");
                    HeatUIEditorHandler.DrawProperty(iconObject, customSkin, "Icon Object");
                    HeatUIEditorHandler.DrawProperty(altIconObject, customSkin, "Alt Icon Object");
                    HeatUIEditorHandler.DrawProperty(textObject, customSkin, "Text Object");
                    HeatUIEditorHandler.DrawProperty(altTextObject, customSkin, "Alt Text Object");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    addPrefix.boolValue = HeatUIEditorHandler.DrawTogglePlain(addPrefix.boolValue, customSkin, "Add Prefix");
                    GUILayout.Space(3);

                    if (addPrefix.boolValue == true)
                        HeatUIEditorHandler.DrawPropertyPlainCW(prefix, customSkin, "Prefix:", 40);

                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    addSuffix.boolValue = HeatUIEditorHandler.DrawTogglePlain(addSuffix.boolValue, customSkin, "Add Suffix");
                    GUILayout.Space(3);

                    if (addSuffix.boolValue == true)
                        HeatUIEditorHandler.DrawPropertyPlainCW(suffix, customSkin, "Suffix:", 40);

                    GUILayout.EndVertical();

                    HeatUIEditorHandler.DrawPropertyCW(decimals, customSkin, "Decimals", 80);


                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Min Limit"), customSkin.FindStyle("Text"), GUILayout.Width(80));
                    EditorGUILayout.PropertyField(minValueLimit, new GUIContent(""));

                    if (minValueLimit.floatValue >= maxValueLimit.floatValue) { minValueLimit.floatValue = maxValueLimit.floatValue - 1; }

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Max Limit"), customSkin.FindStyle("Text"), GUILayout.Width(80));
                    EditorGUILayout.PropertyField(maxValueLimit, new GUIContent(""));

                    if (maxValueLimit.floatValue <= minValue.floatValue) { maxValueLimit.floatValue = minValue.floatValue + 1; }

                    GUILayout.EndHorizontal();

                    HeatUIEditorHandler.DrawPropertyCW(barDirection, customSkin, "Bar Direction", 80);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif