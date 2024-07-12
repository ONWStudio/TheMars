using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Manager
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Onw.Helpers;

    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        public static T Instance => EnsureInstance();
        public static bool HasInstance => _instance != null;

        public static bool ScriptableObjectFileExists
            => HasInstance || File.Exists(GetFilePathWithExtention(true));

        private static T _instance = null;

        public static T EnsureInstance()
        {
            if (_instance == null)
            {
                string filePath = GetFilePathWithExtention(false);
                string resourceFilePath = Path.GetFileNameWithoutExtension(
                        filePath.Split(new string[] { "Resources" }, StringSplitOptions.None).Last());

                if (Resources.Load(resourceFilePath) is not T instance)
                {
                    instance = CreateInstance<T>(); // note: in the debugger it might be displayed as null (which is not the case)

#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
                    string completeFilePath = Path.Combine(Application.dataPath, filePath);
                    string directory = Path.GetDirectoryName(completeFilePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/" + filePath);
                    UnityEditor.AssetDatabase.Refresh();

#else
                    Debug.LogErrorFormat(
                        "Could not find scriptable object of type '{0}'. Make sure it is instantiated inside Unity before building.", 
                        typeof(T));
#endif
                }

                _instance = instance;
            }

            return _instance;
        }

        private static string GetFilePathWithExtention(bool fullPath)
        {
            Type t = typeof(T);
            var prop = t.GetField("FILE_PATH", BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic) ?? throw new Exception($"No static Property 'FilePath' in {t}");

            if (prop.GetValue(null) is not string filePath) throw new Exception($"static property 'FILE_PATH' is not a string or null in {t}");
            if (!filePath.Contains("Resources")) throw new Exception("static property 'FILE_PATH' must contain a Resources folder.");
            if (filePath.Contains("Plugins")) throw new Exception("static property 'FILE_PATH' must not contain a Plugin folder.");

            if (!filePath.EndsWith(".asset"))
            {
                filePath += ".asset";
            }

            return fullPath
                ? Path.Combine(Application.dataPath, filePath)
                : filePath;
        }
    }
}