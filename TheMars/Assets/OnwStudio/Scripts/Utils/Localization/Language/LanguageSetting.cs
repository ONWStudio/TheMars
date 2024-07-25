using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace Onw.Localization
{
    public sealed class LanguageSetting : ScriptableObject
    {
        [field: SerializeField] public string LocalizationID { get; private set; }
        [field: SerializeField] public string LocalizationName { get; private set; }
        [field: SerializeField] public string LocalizedName { get; private set; }

#if UNITY_EDITOR
        [OnChangedValueByMethod(nameof(LocalizationID), nameof(LocalizationName))]
        private void onChangedIDOrName()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(GetInstanceID());
            string objectName = $"({LocalizationID}) {LocalizationName}";

            UnityEditor.AssetDatabase.RenameAsset(assetPath, objectName);
            name = objectName;

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
