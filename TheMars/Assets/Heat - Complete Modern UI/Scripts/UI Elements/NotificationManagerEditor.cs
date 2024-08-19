#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NotificationManager))]
    public class NotificationManagerEditor : Editor
    {
        private NotificationManager nmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            nmTarget = (NotificationManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty icon = serializedObject.FindProperty("icon");
            SerializedProperty notificationText = serializedObject.FindProperty("notificationText");
            SerializedProperty localizationKey = serializedObject.FindProperty("localizationKey");
            SerializedProperty customSFX = serializedObject.FindProperty("customSFX");

            SerializedProperty itemAnimator = serializedObject.FindProperty("itemAnimator");
            SerializedProperty iconObj = serializedObject.FindProperty("iconObj");
            SerializedProperty textObj = serializedObject.FindProperty("textObj");

            SerializedProperty useLocalization = serializedObject.FindProperty("useLocalization");
            SerializedProperty updateOnAnimate = serializedObject.FindProperty("updateOnAnimate");
            SerializedProperty minimizeAfter = serializedObject.FindProperty("minimizeAfter");
            SerializedProperty defaultState = serializedObject.FindProperty("defaultState");
            SerializedProperty afterMinimize = serializedObject.FindProperty("afterMinimize");

            SerializedProperty onDestroy = serializedObject.FindProperty("onDestroy");

            HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
            HeatUIEditorHandler.DrawProperty(icon, customSkin, "Icon");
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField(new GUIContent("Notification Text"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
            EditorGUILayout.PropertyField(notificationText, new GUIContent(""), GUILayout.Height(70));
            GUILayout.EndHorizontal();
            HeatUIEditorHandler.DrawProperty(localizationKey, customSkin, "Localization Key");
            HeatUIEditorHandler.DrawProperty(customSFX, customSkin, "Custom SFX");

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 10);
            HeatUIEditorHandler.DrawProperty(itemAnimator, customSkin, "Animator");
            HeatUIEditorHandler.DrawProperty(iconObj, customSkin, "Icon Object");
            HeatUIEditorHandler.DrawProperty(textObj, customSkin, "Text Object");

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