#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(QuestItem))]
    public class QuestItemEditor : Editor
    {
        private QuestItem qiTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            qiTarget = (QuestItem)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty questText = serializedObject.FindProperty("questText");
            SerializedProperty localizationKey = serializedObject.FindProperty("localizationKey");

            SerializedProperty questAnimator = serializedObject.FindProperty("questAnimator");
            SerializedProperty questTextObj = serializedObject.FindProperty("questTextObj");

            SerializedProperty useLocalization = serializedObject.FindProperty("useLocalization");
            SerializedProperty updateOnAnimate = serializedObject.FindProperty("updateOnAnimate");
            SerializedProperty minimizeAfter = serializedObject.FindProperty("minimizeAfter");
            SerializedProperty defaultState = serializedObject.FindProperty("defaultState");
            SerializedProperty afterMinimize = serializedObject.FindProperty("afterMinimize");

            SerializedProperty onDestroy = serializedObject.FindProperty("onDestroy");

            HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField(new GUIContent("Quest Text"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
            EditorGUILayout.PropertyField(questText, new GUIContent(""), GUILayout.Height(70));
            GUILayout.EndHorizontal();
            HeatUIEditorHandler.DrawProperty(localizationKey, customSkin, "Localization Key");

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 10);
            HeatUIEditorHandler.DrawProperty(questAnimator, customSkin, "Quest Animator");
            HeatUIEditorHandler.DrawProperty(questTextObj, customSkin, "Quest Text Object");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 10);
            useLocalization.boolValue = HeatUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
            updateOnAnimate.boolValue = HeatUIEditorHandler.DrawToggle(updateOnAnimate.boolValue, customSkin, "Update On Animate");
            HeatUIEditorHandler.DrawProperty(minimizeAfter, customSkin, "Minimize After");
            HeatUIEditorHandler.DrawProperty(defaultState, customSkin, "Default State");
            HeatUIEditorHandler.DrawProperty(afterMinimize, customSkin, "After Minimize");

            HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
            EditorGUILayout.PropertyField(onDestroy, new GUIContent("On Destroy"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif