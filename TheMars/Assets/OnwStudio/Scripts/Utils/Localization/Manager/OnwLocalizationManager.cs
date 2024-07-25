using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Manager;
using Onw.Attribute;
using AYellowpaper.SerializedCollections;
using System.IO;

namespace Onw.Localization
{
    public abstract class OnwLocalizationManager<T> : ScriptableObjectSingleton<T> where T : OnwLocalizationManager<T>
    {
        [System.Serializable]
        protected sealed class NestedSerializedDictionary
        {
            [field: SerializeField, SerializedDictionary("Culture Code", "Name"), FormerlySerializedAs("<Names>k__BackingField")]
            public SerializedDictionary<string, string> Names { get; private set; } = new();
        }

        [SerializeField, DisplayAs("선택 언어")] protected LanguageSetting _languageSetting = null;

        protected override sealed void OnAwake()
        {

        }

#if UNITY_EDITOR
        [InspectorButton("Create New LanguageSetting")]
        private void createLanguageSetting()
        {
            LanguageSetting asset = CreateInstance<LanguageSetting>();
            string fullpath = $"Assets/OnwStudio/ScriptableObject/Localization/LocalizationLanguage/{nameof(LanguageSetting)}.asset";
            string directory = Path.GetDirectoryName(fullpath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            UnityEditor.AssetDatabase.CreateAsset(asset, fullpath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            _languageSetting = asset;
        }
#endif
    }
}