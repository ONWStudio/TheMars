#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEditor.AssetDatabase;

namespace TheMarsGUITool
{
    internal sealed partial class TheMarsGUIToolDrawer : EditorWindow
    {
        internal static class DataHandler<T> where T : ScriptableObject
        {
            internal static bool CheckDuplicatedName(string dataPath, string assetName)
                => File.Exists($"{dataPath}/{assetName}.asset");

            internal static T CreateScriptableObject(string dataPath, string assetName)
            {
                T asset = CreateInstance<T>();

                string fullpath = $"{dataPath}/{assetName}.asset";
                string directory = Path.GetDirectoryName(fullpath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                CreateAsset(asset, fullpath);
                SaveAssets();
                Refresh();

                return asset;
            }

            internal static IEnumerable<T> LoadAllScriptableObjects()
                => FindAssets($"t:{typeof(T).Name}")
                    .Select(guid => LoadAssetAtPath<T>(GUIDToAssetPath(guid)));

            internal static void RenameScriptableObject(T asset, string dataPath, string newName)
            {
                string path = $"{dataPath}/{asset.name}.asset";

                RenameAsset(path, newName);
                SaveAssets();
                Refresh();
            }

            internal static void SaveData(T asset)
            {
                EditorUtility.SetDirty(asset);
                SaveAssets();
                Refresh();
            }
        }
    }
}
#endif
