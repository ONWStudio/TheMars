using System.Linq;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Localization;
using TM.Building.Effect;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using TM.Building.Effect.Creator;
using TM.Synergy;

namespace TM.Building
{
    public enum TMCorporation
    {
        A,
        B,
        C,
        D
    }
    
    public sealed class TMBuildingData : ScriptableObject
    {
        public IReadOnlyList<TMSynergyData> Synergies => _synergies;
        
        public string BuildingName => _localizedBuildingName.TryGetLocalizedString(out string buildingName) ? buildingName : "";
        
        [field: SerializeField] public TMCorporation TMCorporation { get; private set; }
        
        [field: SerializeField, DisplayAs("건물 프리팹")] public TMBuilding BuildingPrefab { get; private set; }
        
        [SerializeField, LocalizedString(tableName: "BuildingName"), DisplayAs("건물 이름"), Tooltip("건물 이름은 국가별로 지역화 옵션을 제공합니다")] 
        private LocalizedString _localizedBuildingName;
        
        [FormerlySerializedAs("effectCreators")]
        [SerializeReference, DisplayAs("건물 효과"), Tooltip("건물 효과 리스트"), SerializeReferenceDropdown]
        private List<ITMBuildingEffectCreator> _effectCreators = new();

        [SerializeField, DisplayAs("건물 시너지")]
        private List<TMSynergyData> _synergies = new();

        public IEnumerable<ITMBuildingEffect> CreateBuildingEffects()
        {
            return _effectCreators
                .Select(effectCreator => effectCreator.CreateEffect())
                .ToList();
        }
    }
}
