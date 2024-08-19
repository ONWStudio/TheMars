#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIManagerAudio))]
    public class UIManagerAudioEditor : Editor
    {
        private UIManagerAudio uimaTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            uimaTarget = (UIManagerAudio)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            SerializedProperty audioMixer = serializedObject.FindProperty("audioMixer");
            SerializedProperty audioSource = serializedObject.FindProperty("audioSource");
            SerializedProperty masterSlider = serializedObject.FindProperty("masterSlider");
            SerializedProperty musicSlider = serializedObject.FindProperty("musicSlider");
            SerializedProperty SFXSlider = serializedObject.FindProperty("SFXSlider");
            SerializedProperty UISlider = serializedObject.FindProperty("UISlider");

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
            HeatUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");
            HeatUIEditorHandler.DrawProperty(audioMixer, customSkin, "Audio Mixer");
            HeatUIEditorHandler.DrawProperty(audioSource, customSkin, "Audio Source");
            HeatUIEditorHandler.DrawProperty(masterSlider, customSkin, "Master Slider");
            HeatUIEditorHandler.DrawProperty(musicSlider, customSkin, "Music Slider");
            HeatUIEditorHandler.DrawProperty(SFXSlider, customSkin, "SFX Slider");
            HeatUIEditorHandler.DrawProperty(UISlider, customSkin, "UI Slider");

            if (Application.isPlaying == true)
                return;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif