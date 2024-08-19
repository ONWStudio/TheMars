#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using static Michsky.UI.Heat.BoxButtonManager;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BoxButtonManager))]
    public class BoxButtonManagerEditor : Editor
    {
        private BoxButtonManager buttonTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            buttonTarget = (BoxButtonManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "Button Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            buttonTarget.latestTabIndex = HeatUIEditorHandler.DrawTabs(buttonTarget.latestTabIndex, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                buttonTarget.latestTabIndex = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                buttonTarget.latestTabIndex = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                buttonTarget.latestTabIndex = 2;

            GUILayout.EndHorizontal();

            SerializedProperty animator = serializedObject.FindProperty("animator");
            SerializedProperty audioManager = serializedObject.FindProperty("audioManager");
            SerializedProperty backgroundObj = serializedObject.FindProperty("backgroundObj");
            SerializedProperty iconObj = serializedObject.FindProperty("iconObj");
            SerializedProperty titleObj = serializedObject.FindProperty("titleObj");
            SerializedProperty descriptionObj = serializedObject.FindProperty("descriptionObj");
            SerializedProperty filterObj = serializedObject.FindProperty("filterObj");

            SerializedProperty buttonBackground = serializedObject.FindProperty("buttonBackground");
            SerializedProperty buttonIcon = serializedObject.FindProperty("buttonIcon");
            SerializedProperty buttonTitle = serializedObject.FindProperty("buttonTitle");
            SerializedProperty titleLocalizationKey = serializedObject.FindProperty("titleLocalizationKey");
            SerializedProperty buttonDescription = serializedObject.FindProperty("buttonDescription");
            SerializedProperty descriptionLocalizationKey = serializedObject.FindProperty("descriptionLocalizationKey");
            SerializedProperty backgroundFilter = serializedObject.FindProperty("backgroundFilter");

            SerializedProperty isInteractable = serializedObject.FindProperty("isInteractable");
            SerializedProperty enableBackground = serializedObject.FindProperty("enableBackground");
            SerializedProperty enableIcon = serializedObject.FindProperty("enableIcon");
            SerializedProperty enableTitle = serializedObject.FindProperty("enableTitle");
            SerializedProperty enableDescription = serializedObject.FindProperty("enableDescription");
            SerializedProperty enableFilter = serializedObject.FindProperty("enableFilter");
            SerializedProperty useUINavigation = serializedObject.FindProperty("useUINavigation");
            SerializedProperty navigationMode = serializedObject.FindProperty("navigationMode");
            SerializedProperty wrapAround = serializedObject.FindProperty("wrapAround");
            SerializedProperty selectOnUp = serializedObject.FindProperty("selectOnUp");
            SerializedProperty selectOnDown = serializedObject.FindProperty("selectOnDown");
            SerializedProperty selectOnLeft = serializedObject.FindProperty("selectOnLeft");
            SerializedProperty selectOnRight = serializedObject.FindProperty("selectOnRight");
            SerializedProperty checkForDoubleClick = serializedObject.FindProperty("checkForDoubleClick");
            SerializedProperty useLocalization = serializedObject.FindProperty("useLocalization");
            SerializedProperty useSounds = serializedObject.FindProperty("useSounds");
            SerializedProperty doubleClickPeriod = serializedObject.FindProperty("doubleClickPeriod");
            SerializedProperty useCustomContent = serializedObject.FindProperty("useCustomContent");

            SerializedProperty onClick = serializedObject.FindProperty("onClick");
            SerializedProperty onDoubleClick = serializedObject.FindProperty("onDoubleClick");
            SerializedProperty onHover = serializedObject.FindProperty("onHover");
            SerializedProperty onLeave = serializedObject.FindProperty("onLeave");

            switch (buttonTarget.latestTabIndex)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);

                    if (useCustomContent.boolValue == false)
                    {
                        if (buttonTarget.backgroundObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableBackground.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableBackground.boolValue, customSkin, "Enable Background");

                            GUILayout.Space(4);

                            if (enableBackground.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(buttonBackground, customSkin, "Background", 110);
                            }

                            GUILayout.EndVertical();
                        }

                        if (buttonTarget.iconObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableIcon.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableIcon.boolValue, customSkin, "Enable Icon");

                            GUILayout.Space(4);

                            if (enableIcon.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(buttonIcon, customSkin, "Button Icon", 110);
                            }

                            GUILayout.EndVertical();
                        }

                        if (buttonTarget.titleObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableTitle.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableTitle.boolValue, customSkin, "Enable Title");

                            GUILayout.Space(4);

                            if (enableTitle.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(buttonTitle, customSkin, "Button Text", 110);
                                if (useLocalization.boolValue == true) { HeatUIEditorHandler.DrawPropertyCW(titleLocalizationKey, customSkin, "Localization Key", 110); }
                            }

                            GUILayout.EndVertical();
                        }

                        if (buttonTarget.descriptionObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableDescription.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableDescription.boolValue, customSkin, "Enable Description");

                            GUILayout.Space(4);

                            if (enableDescription.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(buttonDescription, customSkin, "Description", 110);
                                if (useLocalization.boolValue == true) { HeatUIEditorHandler.DrawPropertyCW(descriptionLocalizationKey, customSkin, "Localization Key", 110); }
                            }

                            GUILayout.EndVertical();
                        }

                        if (buttonTarget.filterObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableFilter.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableFilter.boolValue, customSkin, "Enable Hover Filter");

                            GUILayout.Space(4);

                            if (enableFilter.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(backgroundFilter, customSkin, "Background Filter", 110);
                            }

                            GUILayout.EndVertical();
                        }

                        if (Application.isPlaying == false) { buttonTarget.UpdateUI(); }
                    }

                    else { EditorGUILayout.HelpBox("'Use Custom Content' is enabled. Content is now managed manually.", MessageType.Info); }

                    isInteractable.boolValue = HeatUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");

                    if (Application.isPlaying == true && GUILayout.Button("Update UI", customSkin.button)) { buttonTarget.UpdateUI(); }

                    HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
                    EditorGUILayout.PropertyField(onDoubleClick, new GUIContent("On Double Click"), true);
                    EditorGUILayout.PropertyField(onHover, new GUIContent("On Hover"), true);
                    EditorGUILayout.PropertyField(onLeave, new GUIContent("On Leave"), true);
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(backgroundObj, customSkin, "Background Object");
                    HeatUIEditorHandler.DrawProperty(iconObj, customSkin, "Icon Object");
                    HeatUIEditorHandler.DrawProperty(titleObj, customSkin, "Title Object");
                    HeatUIEditorHandler.DrawProperty(descriptionObj, customSkin, "Description Object");
                    HeatUIEditorHandler.DrawProperty(filterObj, customSkin, "Filter Object");
                    HeatUIEditorHandler.DrawProperty(animator, customSkin, "Animator");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    HeatUIEditorHandler.DrawProperty(doubleClickPeriod, customSkin, "Double Click Period");
                    isInteractable.boolValue = HeatUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    useCustomContent.boolValue = HeatUIEditorHandler.DrawToggle(useCustomContent.boolValue, customSkin, "Use Custom Content", "Bypasses inspector values and allows manual editing.");
                    useLocalization.boolValue = HeatUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    GUI.enabled = true;
                    checkForDoubleClick.boolValue = HeatUIEditorHandler.DrawToggle(checkForDoubleClick.boolValue, customSkin, "Check For Double Click");
                    useSounds.boolValue = HeatUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Button Sounds");
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);

                    useUINavigation.boolValue = HeatUIEditorHandler.DrawTogglePlain(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

                    GUILayout.Space(4);

                    if (useUINavigation.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        HeatUIEditorHandler.DrawPropertyPlain(navigationMode, customSkin, "Navigation Mode");

                        if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Horizontal)
                        {
                            EditorGUI.indentLevel = 1;
                            wrapAround.boolValue = HeatUIEditorHandler.DrawToggle(wrapAround.boolValue, customSkin, "Wrap Around");
                            EditorGUI.indentLevel = 0;
                        }

                        else if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Vertical)
                        {
                            wrapAround.boolValue = HeatUIEditorHandler.DrawTogglePlain(wrapAround.boolValue, customSkin, "Wrap Around");
                        }

                        else if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Explicit)
                        {
                            EditorGUI.indentLevel = 1;
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnUp, customSkin, "Select On Up");
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnDown, customSkin, "Select On Down");
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnLeft, customSkin, "Select On Left");
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnRight, customSkin, "Select On Right");
                            EditorGUI.indentLevel = 0;
                        }

                        GUILayout.EndVertical();
                    }

                    GUILayout.EndVertical();
                    buttonTarget.UpdateUI();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif