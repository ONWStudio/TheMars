using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Manager;
using Onw.Attribute;
using Onw.Helpers;
using TMCard.SpecialEffect;
using AYellowpaper.SerializedCollections;

namespace TMCard.Manager
{
    public sealed partial class TMSpecialEffectNameManager : ScriptableObjectSingleton<TMSpecialEffectNameManager>
    {
        [System.Serializable]
        private sealed class NestedSerializedDictionary
        {
            [field: SerializeField, SerializedDictionary("Culture Code", "Name"), FormerlySerializedAs("<EffectNames>k__BackingField")]
            public SerializedDictionary<string, string> EffectNames { get; private set; } = new();
        }    

        private const string FILE_PATH = "OnwStudio/ScriptableObject/Manager/Resources/" + nameof(TMSpecialEffectNameManager);

        [SerializeField, SerializedDictionary("Special Effect", "Name", true, isLocked: true), FormerlySerializedAs("_specialEffectNames"), DisplayAs("특수 효과 이름")]
        [InfoBox("특수 효과는 추가 할 수 없습니다 각 효과 별로 지역별 이름만 추가할 수 있습니다")]
        private SerializedDictionary<string, NestedSerializedDictionary> _specialEffectNames = new(); 

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

                _specialEffectNames.Remove(name);
            }
        }

        public string GetName(string key)
        {
            return _specialEffectNames.TryGetValue(key, out NestedSerializedDictionary localizingNames) &&
                localizingNames.EffectNames.TryGetValue("ko-KR", out string name) ? name : "";
        }
    }
}
