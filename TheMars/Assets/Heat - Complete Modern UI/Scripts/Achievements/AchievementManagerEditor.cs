#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AchievementManager))]
    public class AchievementManagerEditor : Editor
    {
        private AchievementManager amTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            amTarget = (AchievementManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            SerializedProperty allParent = serializedObject.FindProperty("allParent");
            SerializedProperty commonParent = serializedObject.FindProperty("commonParent");
            SerializedProperty rareParent = serializedObject.FindProperty("rareParent");
            SerializedProperty legendaryParent = serializedObject.FindProperty("legendaryParent");
            SerializedProperty achievementPreset = serializedObject.FindProperty("achievementPreset");
            SerializedProperty totalUnlockedObj = serializedObject.FindProperty("totalUnlockedObj");
            SerializedProperty totalValueObj = serializedObject.FindProperty("totalValueObj");
            SerializedProperty commonUnlockedObj = serializedObject.FindProperty("commonUnlockedObj");
            SerializedProperty commonlTotalObj = serializedObject.FindProperty("commonlTotalObj");
            SerializedProperty rareUnlockedObj = serializedObject.FindProperty("rareUnlockedObj");
            SerializedProperty rareTotalObj = serializedObject.FindProperty("rareTotalObj");
            SerializedProperty legendaryUnlockedObj = serializedObject.FindProperty("legendaryUnlockedObj");
            SerializedProperty legendaryTotalObj = serializedObject.FindProperty("legendaryTotalObj");

            SerializedProperty useLocalization = serializedObject.FindProperty("useLocalization");
            SerializedProperty useAlphabeticalOrder = serializedObject.FindProperty("useAlphabeticalOrder");

            HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);
            HeatUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");

            if (amTarget.UIManagerAsset != null)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField(new GUIContent("Library Preset"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                GUI.enabled = false;
                amTarget.UIManagerAsset.achievementLibrary = EditorGUILayout.ObjectField(amTarget.UIManagerAsset.achievementLibrary, typeof(AchievementLibrary), true) as AchievementLibrary;
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            }

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 10);
            HeatUIEditorHandler.DrawProperty(achievementPreset, customSkin, "Achievement Preset");
            HeatUIEditorHandler.DrawProperty(allParent, customSkin, "All Parent");
            HeatUIEditorHandler.DrawProperty(commonParent, customSkin, "Common Parent");
            HeatUIEditorHandler.DrawProperty(rareParent, customSkin, "Rare Parent");
            HeatUIEditorHandler.DrawProperty(legendaryParent, customSkin, "Legendary Parent");
            HeatUIEditorHandler.DrawProperty(totalUnlockedObj, customSkin, "Total Unlocked");
            HeatUIEditorHandler.DrawProperty(totalValueObj, customSkin, "Total Value");
            HeatUIEditorHandler.DrawProperty(commonUnlockedObj, customSkin, "Common Unlocked");
            HeatUIEditorHandler.DrawProperty(commonlTotalObj, customSkin, "Commonl Total");
            HeatUIEditorHandler.DrawProperty(rareUnlockedObj, customSkin, "Rare Unlocked");
            HeatUIEditorHandler.DrawProperty(rareTotalObj, customSkin, "Rare Total");
            HeatUIEditorHandler.DrawProperty(legendaryUnlockedObj, customSkin, "Legendary Unlocked");
            HeatUIEditorHandler.DrawProperty(legendaryTotalObj, customSkin, "Legendary Total");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 10);
            useLocalization.boolValue = HeatUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
            useAlphabeticalOrder.boolValue = HeatUIEditorHandler.DrawToggle(useAlphabeticalOrder.boolValue, customSkin, "Use Alphabetical Order");

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif