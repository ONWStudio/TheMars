using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using UnityEditor;
using UnityEditor.VersionControl;

namespace Onw.Localization
{
    public sealed class LanguageSetting : ScriptableObject
    {
        [field: SerializeField] public string LocalizationID { get; private set; }
        [field: SerializeField] public string LocalizationName { get; private set; }
        [field: SerializeField] public string LocalizedName { get; private set; }

#if UNITY_EDITOR
        [OnValueChangedByMethod(nameof(LocalizationID), nameof(LocalizationName))]
        private void onChangedIDOrName()
        {
            Debug.Log("asdf");

            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, $"({LocalizationID}) {LocalizationName}");
        }
#endif
    }
}
