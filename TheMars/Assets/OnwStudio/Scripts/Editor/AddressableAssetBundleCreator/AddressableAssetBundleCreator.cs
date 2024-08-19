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
        public static void CreateAssetBundle(string address, string groupName, Object @object, params string[] labels)
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
            if (entry != null)
            {
                entry.address = address;

                foreach (string label in labels)
                {
                    if (!settings.GetLabels().Contains(label))
                    {
                        settings.AddLabel(label);
                    }
                    entry.SetLabel(label, true, true);
                }
            }

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();

            Debug.Log("Asset added to Addressable Group successfully.");
        }

        public static void RemoveAssetBundle(string address, string groupName)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found.");
                return;
            }

            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null)
            {
                Debug.LogError($"Group {groupName} not found.");
                return;
            }

            foreach (AddressableAssetEntry entry in group.entries)
            {
                if (entry.address != address) continue;

                settings.RemoveAssetEntry(entry.guid);
                Debug.Log($"Asset with address {address} removed from group {groupName}");
                return;
            }

            Debug.LogError($"Asset with address {address} not found in group {groupName}");
        }
    }
}
#endif