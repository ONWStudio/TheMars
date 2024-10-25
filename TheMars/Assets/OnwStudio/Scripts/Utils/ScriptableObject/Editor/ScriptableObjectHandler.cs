#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.ScriptableObject;
using static UnityEditor.AssetDatabase;

namespace Onw.ScriptableObjects.Editor
{
    public static class ScriptableObjectHandler
    {
        public static bool CheckDuplicatedName(string dataPath, string assetName)
            => File.Exists($"{dataPath}/{assetName}.asset");

        public static T CreateScriptableObject<T>(string dataPath, string assetName) where T : ScriptableObject
        {
            T asset = CreateInstance<T>();
            string fullPath = $"{dataPath}/{assetName}.asset";
            string directory = Path.GetDirectoryName(fullPath)!;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            CreateAsset(asset, fullPath);
            SaveData(asset);

            return asset;
        }

        public static ScriptableObject CreateScriptableObject(string dataPath, string assetName, Type type)
        {
            if (!type.IsSubclassOf(typeof(ScriptableObject))) return null;

            ScriptableObject asset = CreateInstance(type);
            string fullPath = $"{dataPath}/{assetName}.asset";
            string directory = Path.GetDirectoryName(fullPath)!;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            CreateAsset(asset, fullPath);
            SaveData(asset);
            
            return asset;
        }

        public static IEnumerable<ScriptableObject> LoadAllScriptableObjects(Type type)
        {
            return FindAssets($"t:{type.Name}")
                .Select(GUIDToAssetPath)
                .Select(LoadAssetAtPath<ScriptableObject>)
                .Where(so => so);
        }

        public static IEnumerable<T> LoadAllScriptableObjects<T>() where T : ScriptableObject 
            => FindAssets($"t:{typeof(T).Name}")
                .Select(guid => LoadAssetAtPath<T>(GUIDToAssetPath(guid)));

        public static void RenameScriptableObject(ScriptableObject asset, string newName)
        {
            RenameAsset(GetAssetPath(asset), newName);
            SaveData(asset);
        }

        public static void SaveData(ScriptableObject asset)
        {
            EditorUtility.SetDirty(asset);
            SaveAssets();
            Refresh();
        }
    }
}
#endif