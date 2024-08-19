#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(UIManagerText))]
    public class UIManagerTextEditor : Editor
    {
        private UIManagerText uimtTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            uimtTarget = (UIManagerText)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            SerializedProperty fontType = serializedObject.FindProperty("fontType");
            SerializedProperty colorType = serializedObject.FindProperty("colorType");
            SerializedProperty useCustomColor = serializedObject.FindProperty("useCustomColor");
            SerializedProperty useCustomAlpha = serializedObject.FindProperty("useCustomAlpha");
            SerializedProperty useCustomFont = serializedObject.FindProperty("useCustomFont");

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
            HeatUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 10);

            if (uimtTarget.UIManagerAsset != null)
            {
                if (useCustomFont.boolValue == true) { GUI.enabled = false; }
                HeatUIEditorHandler.DrawProperty(fontType, customSkin, "Font Type");
                GUI.enabled = true;
                HeatUIEditorHandler.DrawProperty(colorType, customSkin, "Color Type");
                useCustomColor.boolValue = HeatUIEditorHandler.DrawToggle(useCustomColor.boolValue, customSkin, "Use Custom Color");
                if (useCustomColor.boolValue == true) { GUI.enabled = false; }
                useCustomAlpha.boolValue = HeatUIEditorHandler.DrawToggle(useCustomAlpha.boolValue, customSkin, "Use Custom Alpha");
                GUI.enabled = true;
                useCustomFont.boolValue = HeatUIEditorHandler.DrawToggle(useCustomFont.boolValue, customSkin, "Use Custom Font");
            }

            else { EditorGUILayout.HelpBox("UI Manager should be assigned.", MessageType.Error); }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif