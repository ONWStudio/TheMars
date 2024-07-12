using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
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
        private const string FILE_PATH = "OnwStudio/ScriptableObject/Manager/Resources/" + nameof(TMSpecialEffectNameManager);

        [SerializeField, SerializedDictionary("Special Effect", "Name", true, isLooked: true), FormerlySerializedAs("_specialEffectNames"), DisplayAs("특수 효과 이름")]
        [InfoBox("특수 효과는 추가 할 수 없습니다 각 효과 별로 지역별 이름만 추가할 수 있습니다")]
        private SerializedDictionary<string, SerializedDictionary<string, string>> _specialEffectNames = new(); 

        private void OnEnable()
        {
            foreach (string subClassName in ReflectionHelper.GetClassNamesFromParent<ICardSpecialEffect>())
            {
                if (_specialEffectNames.ContainsKey(subClassName)) continue;

                _specialEffectNames.NewAdd(subClassName, new SerializedDictionary<string, string>());
            }
        }
    }
}
