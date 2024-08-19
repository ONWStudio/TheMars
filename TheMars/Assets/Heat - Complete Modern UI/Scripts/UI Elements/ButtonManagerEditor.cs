#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ButtonManager))]
    public class ButtonManagerEditor : Editor
    {
        private ButtonManager buttonTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            buttonTarget = (ButtonManager)target;

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

            SerializedProperty normalCG = serializedObject.FindProperty("normalCG");
            SerializedProperty highlightCG = serializedObject.FindProperty("highlightCG");
            SerializedProperty disabledCG = serializedObject.FindProperty("disabledCG");
            SerializedProperty normalTextObj = serializedObject.FindProperty("normalTextObj");
            SerializedProperty highlightTextObj = serializedObject.FindProperty("highlightTextObj");
            SerializedProperty disabledTextObj = serializedObject.FindProperty("disabledTextObj");
            SerializedProperty normalImageObj = serializedObject.FindProperty("normalImageObj");
            SerializedProperty highlightImageObj = serializedObject.FindProperty("highlightImageObj");
            SerializedProperty disabledImageObj = serializedObject.FindProperty("disabledImageObj");

            SerializedProperty buttonIcon = serializedObject.FindProperty("buttonIcon");
            SerializedProperty buttonText = serializedObject.FindProperty("buttonText");
            SerializedProperty iconScale = serializedObject.FindProperty("iconScale");
            SerializedProperty textSize = serializedObject.FindProperty("textSize");

            SerializedProperty autoFitContent = serializedObject.FindProperty("autoFitContent");
            SerializedProperty padding = serializedObject.FindProperty("padding");
            SerializedProperty spacing = serializedObject.FindProperty("spacing");
            SerializedProperty disabledLayout = serializedObject.FindProperty("disabledLayout");
            SerializedProperty normalLayout = serializedObject.FindProperty("normalLayout");
            SerializedProperty highlightedLayout = serializedObject.FindProperty("highlightedLayout");
            SerializedProperty mainLayout = serializedObject.FindProperty("mainLayout");
            SerializedProperty mainFitter = serializedObject.FindProperty("mainFitter");
            SerializedProperty targetFitter = serializedObject.FindProperty("targetFitter");
            SerializedProperty targetRect = serializedObject.FindProperty("targetRect");

            SerializedProperty isInteractable = serializedObject.FindProperty("isInteractable");
            SerializedProperty enableIcon = serializedObject.FindProperty("enableIcon");
            SerializedProperty enableText = serializedObject.FindProperty("enableText");
            SerializedProperty useCustomTextSize = serializedObject.FindProperty("useCustomTextSize");
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
            SerializedProperty fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");
            SerializedProperty useCustomContent = serializedObject.FindProperty("useCustomContent");
            SerializedProperty bypassControllerManager = serializedObject.FindProperty("bypassControllerManager");

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
                        if (buttonTarget.normalImageObj != null || buttonTarget.highlightImageObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableIcon.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableIcon.boolValue, customSkin, "Enable Icon");

                            GUILayout.Space(4);

                            if (enableIcon.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(buttonIcon, customSkin, "Button Icon", 80);
                                HeatUIEditorHandler.DrawPropertyCW(iconScale, customSkin, "Icon Scale", 80);
                                if (enableText.boolValue == true) { HeatUIEditorHandler.DrawPropertyCW(spacing, customSkin, "Spacing", 80); }
                            }

                            GUILayout.EndVertical();
                        }

                        if (buttonTarget.normalTextObj != null || buttonTarget.highlightTextObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableText.boolValue = HeatUIEditorHandler.DrawTogglePlain(enableText.boolValue, customSkin, "Enable Text");

                            GUILayout.Space(4);

                            if (enableText.boolValue == true)
                            {
                                HeatUIEditorHandler.DrawPropertyCW(buttonText, customSkin, "Button Text", 80);
                                if (useCustomTextSize.boolValue == false) { HeatUIEditorHandler.DrawPropertyCW(textSize, customSkin, "Text Size", 80); }
                            }

                            GUILayout.EndVertical();
                        }

                        if (Application.isPlaying == false) { buttonTarget.UpdateUI(); }
                    }

                    else { EditorGUILayout.HelpBox("'Use Custom Content' is enabled. Content is now managed manually.", MessageType.Info); }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);

                    autoFitContent.boolValue = HeatUIEditorHandler.DrawTogglePlain(autoFitContent.boolValue, customSkin, "Auto-Fit Content", "Sets the width based on the button content.");

                    GUILayout.Space(4);

                    if (autoFitContent.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUI.indentLevel = 1;
                        EditorGUILayout.PropertyField(padding, new GUIContent(" Padding"), true);
                        EditorGUI.indentLevel = 0;
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();

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
                    HeatUIEditorHandler.DrawProperty(normalCG, customSkin, "Normal CG");
                    HeatUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");
                    HeatUIEditorHandler.DrawProperty(disabledCG, customSkin, "Disabled CG");

                    if (enableText.boolValue == true)
                    {
                        HeatUIEditorHandler.DrawProperty(normalTextObj, customSkin, "Normal Text");
                        HeatUIEditorHandler.DrawProperty(highlightTextObj, customSkin, "Highlighted Text");
                        HeatUIEditorHandler.DrawProperty(disabledTextObj, customSkin, "Disabled Text");
                    }

                    if (enableIcon.boolValue == true)
                    {
                        HeatUIEditorHandler.DrawProperty(normalImageObj, customSkin, "Normal Icon");
                        HeatUIEditorHandler.DrawProperty(highlightImageObj, customSkin, "Highlight Icon");
                        HeatUIEditorHandler.DrawProperty(disabledImageObj, customSkin, "Disabled Icon");
                    }

                    HeatUIEditorHandler.DrawProperty(disabledLayout, customSkin, "Disabled Layout");
                    HeatUIEditorHandler.DrawProperty(normalLayout, customSkin, "Normal Layout");
                    HeatUIEditorHandler.DrawProperty(highlightedLayout, customSkin, "Highlighted Layout");
                    HeatUIEditorHandler.DrawProperty(mainLayout, customSkin, "Main Layout");

                    if (autoFitContent.boolValue == true)
                    {
                        HeatUIEditorHandler.DrawProperty(mainFitter, customSkin, "Main Fitter");
                        HeatUIEditorHandler.DrawProperty(targetFitter, customSkin, "Target Fitter");
                        HeatUIEditorHandler.DrawProperty(targetRect, customSkin, "Target Rect");
                    }

                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    HeatUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");
                    HeatUIEditorHandler.DrawProperty(doubleClickPeriod, customSkin, "Double Click Period");
                    isInteractable.boolValue = HeatUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    useCustomContent.boolValue = HeatUIEditorHandler.DrawToggle(useCustomContent.boolValue, customSkin, "Use Custom Content", "Bypasses inspector values and allows manual editing.");
                    if (useCustomContent.boolValue == true || enableText.boolValue == false) { GUI.enabled = false; }
                    useCustomTextSize.boolValue = HeatUIEditorHandler.DrawToggle(useCustomTextSize.boolValue, customSkin, "Use Custom Text Size");
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