#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(UIGradient))]
    public class UIGradientEditor : Editor
    {
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty _effectGradient = serializedObject.FindProperty("_effectGradient");
            SerializedProperty _gradientType = serializedObject.FindProperty("_gradientType");
            SerializedProperty _offset = serializedObject.FindProperty("_offset");
            SerializedProperty _zoom = serializedObject.FindProperty("_zoom");
            SerializedProperty _modifyVertices = serializedObject.FindProperty("_modifyVertices");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
            HeatUIEditorHandler.DrawPropertyCW(_effectGradient, customSkin, "Gradient", 100);
            HeatUIEditorHandler.DrawPropertyCW(_gradientType, customSkin, "Type", 100);
            HeatUIEditorHandler.DrawPropertyCW(_offset, customSkin, "Offset", 100);
            HeatUIEditorHandler.DrawPropertyCW(_zoom, customSkin, "Zoom", 100);
            _modifyVertices.boolValue = HeatUIEditorHandler.DrawToggle(_modifyVertices.boolValue, customSkin, "Complex Gradient");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif