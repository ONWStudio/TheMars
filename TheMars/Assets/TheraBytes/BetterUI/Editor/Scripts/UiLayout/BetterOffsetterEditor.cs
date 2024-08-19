using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi.Editor
{
    [CustomEditor(typeof(BetterOffsetter)), CanEditMultipleObjects]
    public class BetterOffsetterEditor : UnityEditor.Editor
    {
        private InjectedSettingsInspector helper;

        private void OnEnable()
        {
            helper = new InjectedSettingsInspector("Settings", serializedObject, "customSettings", "settingsFallback");

            helper.Register("Anchored Position X", "applyPosX",
                "customAnchorPosXSizers", "anchorPosXSizerFallback");

            helper.Register("Anchored Position Y", "applyPosY",
                "customAnchorPosYSizers", "anchorPosYSizerFallback");

            helper.RegisterSpace();

            helper.Register("Size Delta X", "applySizeX",
                "customSizeDeltaXSizers", "sizeDeltaXSizerFallback");

            helper.Register("Size Delta Y", "applySizeY",
                "customSizeDeltaYSizers", "sizeDeltaYSizerFallback");
        }

        public override void OnInspectorGUI()
        {
            helper.Draw();
        }

        [MenuItem("CONTEXT/RectTransform/♠ Add Better Offsetter", false)]
        public static void AddBetterOffsetter(MenuCommand command)
        {
            RectTransform ctx = command.context as RectTransform;
            AddBetterOffsetter(ctx);
        }

        private static BetterOffsetter AddBetterOffsetter(Transform transform)
        {
            BetterOffsetter offsetter = transform.gameObject.AddComponent<BetterOffsetter>();

            while (UnityEditorInternal.ComponentUtility.MoveComponentUp(offsetter))
            { }

            if (transform.gameObject.GetComponent<BetterLocator>() != null)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(offsetter);
            }

            return offsetter;
        }

        [MenuItem("CONTEXT/RectTransform/♠ Add Better Offsetter", true)]
        public static bool CheckBetterOffsetter(MenuCommand command)
        {
            RectTransform ctx = command.context as RectTransform;
            return ctx.gameObject.GetComponent<BetterOffsetter>() == null;
        }

        [MenuItem("CONTEXT/SizeDeltaSizer/♠ Convert to Better Offsetter")]
        public static void ConvertToBetterOffsetter(MenuCommand command)
        {
            SizeDeltaSizer ctx = command.context as SizeDeltaSizer;
        }

        public static void ConvertToBetterOffsetter(SizeDeltaSizer sizer)
        { 
            BetterOffsetter offsetter = sizer.gameObject.GetComponent<BetterOffsetter>();

            if (offsetter == null)
            {
                offsetter = AddBetterOffsetter(sizer.transform);
            }

            // size delta sizer fields
            FieldInfo settingsCollectionField = sizer.GetType()
                .GetField("customSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            SizeDeltaSizer.SettingsConfigCollection settingsCollection = settingsCollectionField.GetValue(sizer) as SizeDeltaSizer.SettingsConfigCollection;

            FieldInfo settingsFallbackField = sizer.GetType()
                .GetField("settingsFallback", BindingFlags.NonPublic | BindingFlags.Instance);
            SizeDeltaSizer.Settings settingsFallback = settingsFallbackField.GetValue(sizer) as SizeDeltaSizer.Settings;


            FieldInfo sizeDeltaCollectionField = sizer.GetType()
                .GetField("customDeltaSizers", BindingFlags.NonPublic | BindingFlags.Instance);
            Vector2SizeConfigCollection sizeDeltaCollection = sizeDeltaCollectionField.GetValue(sizer) as Vector2SizeConfigCollection;

            FieldInfo sizeDeltaFallbackField = sizer.GetType()
                .GetField("deltaSizerFallback", BindingFlags.NonPublic | BindingFlags.Instance);
            Vector2SizeModifier sizeDeltaFallback = sizeDeltaFallbackField.GetValue(sizer) as Vector2SizeModifier;

            //offsetter fields
            FieldInfo sizeXField = offsetter.GetType()
                .GetField("customSizeDeltaXSizers", BindingFlags.NonPublic | BindingFlags.Instance);
            FloatSizeConfigCollection sizeXObj = sizeXField.GetValue(offsetter) as FloatSizeConfigCollection;

            FieldInfo sizeYField = offsetter.GetType()
                .GetField("customSizeDeltaYSizers", BindingFlags.NonPublic | BindingFlags.Instance);
            FloatSizeConfigCollection sizeYObj = sizeYField.GetValue(offsetter) as FloatSizeConfigCollection;

            FieldInfo offsetterSettingsCollectionField = offsetter.GetType()
                .GetField("customSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            BetterOffsetter.SettingsConfigCollection offsetterSettingsCollection = offsetterSettingsCollectionField.GetValue(offsetter) as BetterOffsetter.SettingsConfigCollection;

            FieldInfo offsetterSettingsFallbackField = offsetter.GetType()
                .GetField("settingsFallback", BindingFlags.NonPublic | BindingFlags.Instance);

            FieldInfo offsetterSizeXFallbackField = offsetter.GetType()
                .GetField("sizeDeltaXSizerFallback", BindingFlags.NonPublic | BindingFlags.Instance);

            FieldInfo offsetterSizeYFallbackField = offsetter.GetType()
                .GetField("sizeDeltaYSizerFallback", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (SizeDeltaSizer.Settings sizeDeltaSettings in settingsCollection.Items)
            {
                Vector2SizeModifier deltaSizer = sizeDeltaCollection.GetItemForConfig(sizeDeltaSettings.ScreenConfigName, sizeDeltaFallback);

                CopyOverSizeDeltaSizerModifiers(sizeDeltaSettings, deltaSizer,
                    out BetterOffsetter.Settings offsetterSettings, out FloatSizeModifier sizeModX, out FloatSizeModifier sizeModY);

                offsetterSettingsCollection.AddItem(offsetterSettings);
                sizeXObj.AddItem(sizeModX);
                sizeYObj.AddItem(sizeModY);
            }

            // fallback
            {
                CopyOverSizeDeltaSizerModifiers(settingsFallback, sizeDeltaFallback,
                    out BetterOffsetter.Settings offsetterSettings, out FloatSizeModifier sizeModX, out FloatSizeModifier sizeModY);

                offsetterSettingsFallbackField.SetValue(offsetter, offsetterSettings);
                offsetterSizeXFallbackField.SetValue(offsetter, sizeModX);
                offsetterSizeYFallbackField.SetValue(offsetter, sizeModY);
            }

            GameObject.DestroyImmediate(sizer);
        }

        private static void CopyOverSizeDeltaSizerModifiers(
            SizeDeltaSizer.Settings sizeDeltaSettings,
            Vector2SizeModifier deltaSizer,
            out BetterOffsetter.Settings offsetterSettings,
            out FloatSizeModifier sizeXMod,
            out FloatSizeModifier sizeYMod)
        {
            offsetterSettings = new BetterOffsetter.Settings();
            offsetterSettings.ScreenConfigName = sizeDeltaSettings.ScreenConfigName;
            offsetterSettings.ApplySizeX = sizeDeltaSettings.ApplyWidth;
            offsetterSettings.ApplySizeY = sizeDeltaSettings.ApplyHeight;

            sizeXMod = new FloatSizeModifier(deltaSizer.OptimizedSize.x, deltaSizer.MinSize.x, deltaSizer.MaxSize.x)
            {
                ScreenConfigName = sizeDeltaSettings.ScreenConfigName
            };

            sizeYMod = new FloatSizeModifier(deltaSizer.OptimizedSize.y, deltaSizer.MinSize.y, deltaSizer.MaxSize.y)
            {
                ScreenConfigName = sizeDeltaSettings.ScreenConfigName
            };
        }
    }
}
