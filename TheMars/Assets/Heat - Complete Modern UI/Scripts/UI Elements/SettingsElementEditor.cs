#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingsElement))]
    public class SettingsElementEditor : Editor
    {
        private SettingsElement seTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            seTarget = (SettingsElement)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty highlightCG = serializedObject.FindProperty("highlightCG");

            SerializedProperty isInteractable = serializedObject.FindProperty("isInteractable");
            SerializedProperty useSounds = serializedObject.FindProperty("useSounds");
            SerializedProperty useUINavigation = serializedObject.FindProperty("useUINavigation");
            SerializedProperty navigationMode = serializedObject.FindProperty("navigationMode");
            SerializedProperty selectOnUp = serializedObject.FindProperty("selectOnUp");
            SerializedProperty selectOnDown = serializedObject.FindProperty("selectOnDown");
            SerializedProperty selectOnLeft = serializedObject.FindProperty("selectOnLeft");
            SerializedProperty selectOnRight = serializedObject.FindProperty("selectOnRight");
            SerializedProperty wrapAround = serializedObject.FindProperty("wrapAround");
            SerializedProperty fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");

            SerializedProperty onClick = serializedObject.FindProperty("onClick");
            SerializedProperty onHover = serializedObject.FindProperty("onHover");
            SerializedProperty onLeave = serializedObject.FindProperty("onLeave");

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
            HeatUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 10);
            isInteractable.boolValue = HeatUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
            useSounds.boolValue = HeatUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Sounds");

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(-3);

            useUINavigation.boolValue = HeatUIEditorHandler.DrawTogglePlain(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

            GUILayout.Space(4);

            if (useUINavigation.boolValue == true)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                HeatUIEditorHandler.DrawPropertyPlain(navigationMode, customSkin, "Navigation Mode");

                if (seTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Horizontal)
                {
                    EditorGUI.indentLevel = 1;
                    wrapAround.boolValue = HeatUIEditorHandler.DrawToggle(wrapAround.boolValue, customSkin, "Wrap Around");
                    EditorGUI.indentLevel = 0;
                }

                else if (seTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Vertical)
                {
                    wrapAround.boolValue = HeatUIEditorHandler.DrawTogglePlain(wrapAround.boolValue, customSkin, "Wrap Around");
                }

                else if (seTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Explicit)
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
            HeatUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");

            HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
            EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
            EditorGUILayout.PropertyField(onHover, new GUIContent("On Hover"), true);
            EditorGUILayout.PropertyField(onLeave, new GUIContent("On Leave"), true);

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif