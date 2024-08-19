#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.ScriptableObject;
using static UnityEditor.AssetDatabase;

namespace Onw.ScriptableObjects.Editor
{
    public static class ScriptableObjectHandler<T> where T : ScriptableObject
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
            SaveData(asset);

            return asset;
        }

        public static IEnumerable<T> LoadAllScriptableObjects()
            => FindAssets($"t:{typeof(T).Name}")
                .Select(guid => LoadAssetAtPath<T>(GUIDToAssetPath(guid)));

        public static void RenameScriptableObject(T asset, string newName)
        {
            RenameAsset(GetAssetPath(asset), newName);
            SaveData(asset);
        }

        public static void SaveData(T asset)
        {
            EditorUtility.SetDirty(asset);
            SaveAssets();
            Refresh();
        }
    }
}
#endif