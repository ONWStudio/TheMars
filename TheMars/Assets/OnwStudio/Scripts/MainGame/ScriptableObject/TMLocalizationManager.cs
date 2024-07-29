using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Localization;
using Onw.Extensions;
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

        [SerializeField, SerializedDictionary("Card Description", "Description", true, isLocked: true), FormerlySerializedAs("_cardDescriptions"), DisplayAs("카드 설명")]
        private SerializedDictionary<string, NestedSerializedDictionary> _cardDescriptions = new();

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Initialize()
        {
            _ = Instance;
        }
#endif

        private void OnEnable()
        {
#if UNITY_EDITOR
            #region 특수 효과
            var subClassNames = ReflectionHelper.GetCustomLabelFromNestedClass<ITMSpecialEffectCreator>();

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
            #endregion

            var cardDataList = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(TMCardData).Name}")
                .Select(guid => UnityEditor.AssetDatabase.LoadAssetAtPath<TMCardData>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            #region 카드 이름
            foreach (TMCardData cardData in cardDataList)
            {
                if (_cardNames.ContainsKey(cardData.name)) continue;

                _cardNames.NewAdd(cardData.name, new NestedSerializedDictionary());
            }

            foreach (var kvp in _cardNames)
            {
                if (cardDataList.Any(cardData => cardData.name == kvp.Key)) continue;

                _cardNames.NewRemove(kvp.Key, kvp.Value);
            }
            #endregion
            #region 카드 설명
            foreach (TMCardData cardData in cardDataList)
            {
                if (_cardDescriptions.ContainsKey(cardData.name)) continue;

                _cardDescriptions.NewAdd(cardData.name, new NestedSerializedDictionary());
            }

            foreach (var kvp in _cardDescriptions)
            {
                if (cardDataList.Any(cardData => cardData.name == kvp.Key)) continue;

                _cardDescriptions.NewRemove(kvp.Key, kvp.Value);
            }
            #endregion

            _specialEffectLabels.Values.ForEach(SetLocalization);
            _cardNames.Values.ForEach(SetLocalization);
            _cardDescriptions.Values.ForEach(SetLocalization);
#endif
        }

        public string GetDescription(string stackID)
        {
            return _cardDescriptions.TryGetValue(stackID, out NestedSerializedDictionary localizationDescription) &&
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
