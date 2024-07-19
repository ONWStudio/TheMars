using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Localization;
using Onw.Attribute;
using Onw.Helpers;
using TMCard.SpecialEffect;
using AYellowpaper.SerializedCollections;

namespace TMCard
{
    public sealed class TMLocalizationManager : OnwLocalizationManager<TMLocalizationManager>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/Localization/Resources/" + nameof(TMLocalizationManager);

        [SerializeField, SerializedDictionary("Special Effect", "Name", true, isLocked: true), FormerlySerializedAs("_specialEffectNames"), DisplayAs("특수 효과 이름")]
        private SerializedDictionary<string, NestedSerializedDictionary> _specialEffectNames = new();

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
            var subClassNames = ReflectionHelper.GetClassNamesFromParent<ITMCardSpecialEffect>();

            foreach (string subClassName in subClassNames)
            {
                if (_specialEffectNames.ContainsKey(subClassName)) continue;

                _specialEffectNames.NewAdd(subClassName, new NestedSerializedDictionary());
            }

            foreach (string name in _specialEffectNames.Keys)
            {
                if (subClassNames.Any(subClassName => subClassName == name)) continue;

                _specialEffectNames.NewRemove(name);
            }
        }

        public string GetSpecialCardName(string key)
        {
            return _specialEffectNames.TryGetValue(key, out NestedSerializedDictionary localizingNames) &&
                localizingNames.Names.TryGetValue("ko-KR", out string name) ? name : "";
        }
    }
}
