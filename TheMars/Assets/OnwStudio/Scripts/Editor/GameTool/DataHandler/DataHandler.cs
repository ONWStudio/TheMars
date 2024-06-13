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
        private static class DataHandler<T> where T : ScriptableObject
        {
            public static bool CheckDuplicatedName(string dataPath, string assetName)
                => File.Exists($"{dataPath}/{assetName}.asset");

            public static T CreateScriptableObject(string dataPath, string assetName)
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

            public static IEnumerable<T> LoadAllScriptableObjects()
                => FindAssets($"t:{typeof(T).Name}")
                    .Select(guid => LoadAssetAtPath<T>(GUIDToAssetPath(guid)));

            public static void RenameScriptableObject(T asset, string dataPath, string newName)
            {
                string path = $"{dataPath}/{asset.name}.asset";

                RenameAsset(path, newName);
                SaveAssets();
                Refresh();
            }

            public static void SaveData(T asset)
            {
                EditorUtility.SetDirty(asset);
                SaveAssets();
                Refresh();
            }
        }
    }
}
#endif
