using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Localization;
using Onw.Attribute;
using Onw.Helpers;
using TMCard.Effect;
using AYellowpaper.SerializedCollections;

namespace TMCard
{
    public sealed class TMLocalizationManager : OnwLocalizationManager<TMLocalizationManager>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/Localization/Resources/" + nameof(TMLocalizationManager);

        [SerializeField, SerializedDictionary("Special Effect", "Name", true, isLocked: true), FormerlySerializedAs("_specialEffectNames"), DisplayAs("특수 효과 이름")]
        private SerializedDictionary<string, NestedSerializedDictionary> _specialEffectLabels = new();

        [SerializeField, SerializedDictionary("Card Name", "Name", true, isLocked: true), FormerlySerializedAs("_cardNames"), DisplayAs("카드 이름")]
        private SerializedDictionary<string, NestedSerializedDictionary> _cardNames = new();

        [SerializeField, SerializedDictionary("Card Description", "Description", isLocked: true), FormerlySerializedAs("_cardDescription"), DisplayAs("카드 설명")]
        private SerializedDictionary<string, NestedSerializedDictionary> _cardDescription = new();

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Initialize()
        {
            _ = Instance;
        }
#endif

        private void OnEnable()
        {
            //_specialEffectLabels.Clear();

            var subClassNames = ReflectionHelper.GetCustomLabelFromNestedClass<ITMCardSpecialEffect>();

            foreach (string subClassName in subClassNames)
            {
                if (_specialEffectLabels.ContainsKey(subClassName)) continue;

                _specialEffectLabels.NewAdd(subClassName, new NestedSerializedDictionary());
            }

            foreach (var kvp in _specialEffectLabels)
            {
                if (subClassNames.Any(subClassName => subClassName == kvp.Key)) continue;

                _specialEffectLabels.NewRemove(kvp.Key, kvp.Value);
            }
        }

        public string GetDescription(string stackID)
        {
            return _cardDescription.TryGetValue(stackID, out NestedSerializedDictionary localizationDescription) &&
                localizationDescription.Names.TryGetValue(_languageSetting.LocalizationID, out string description) ? description : "";
        }

        public string GetCardName(string stackID)
        {
            return _cardNames.TryGetValue(stackID, out NestedSerializedDictionary localizationNames) &&
                localizationNames.Names.TryGetValue(_languageSetting.LocalizationID, out string name) ? name : "";
        }

        public string GetSpecialEffectLabel(string key)
        {
            return _specialEffectLabels.TryGetValue(key, out NestedSerializedDictionary localizingNames) &&
                localizingNames.Names.TryGetValue(_languageSetting.LocalizationID, out string name) ? name : "";
        }
    }
}
