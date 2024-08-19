#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NavigationBar))]
    public class NavigationBarEditor : Editor
    {
        private NavigationBar navTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            navTarget = (NavigationBar)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty animator = serializedObject.FindProperty("animator");
            SerializedProperty canvasGroup = serializedObject.FindProperty("canvasGroup");

            SerializedProperty updateMode = serializedObject.FindProperty("updateMode");
            SerializedProperty barDirection = serializedObject.FindProperty("barDirection");
            SerializedProperty fadeButtons = serializedObject.FindProperty("fadeButtons");
            SerializedProperty buttonParent = serializedObject.FindProperty("buttonParent");
      
            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
            HeatUIEditorHandler.DrawProperty(animator, customSkin, "Animator");
            HeatUIEditorHandler.DrawProperty(canvasGroup, customSkin, "Canvas Group");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 10);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(-3);
            fadeButtons.boolValue = HeatUIEditorHandler.DrawTogglePlain(fadeButtons.boolValue, customSkin, "Fade Panel Buttons");
            GUILayout.Space(4);
            if (fadeButtons.boolValue == true) { HeatUIEditorHandler.DrawProperty(buttonParent, customSkin, "Button Parent"); }
            GUILayout.EndVertical();
            HeatUIEditorHandler.DrawProperty(updateMode, customSkin, "Update Mode");
            HeatUIEditorHandler.DrawProperty(barDirection, customSkin, "Bar Direction");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif