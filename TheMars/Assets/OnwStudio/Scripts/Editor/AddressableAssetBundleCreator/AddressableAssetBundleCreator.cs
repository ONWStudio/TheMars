#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace AddressableAssetBundleSpace
{
    public static class AddressableAssetBundleCreator
    {
        public static void CreateAssetBundle(string referenceName, string groupName, Object @object)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings == null)
            {
                Debug.LogError("Addressable Asset Settings not found.");
                return;
            }

            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema>());
                group.AddSchema<BundledAssetGroupSchema>();
            }

            string assetPath = AssetDatabase.GetAssetPath(@object);
            string guid = AssetDatabase.AssetPathToGUID(assetPath);

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = referenceName;

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();

            Debug.Log("Asset added to Addressable Group successfully.");
        }
    }
}
#endif