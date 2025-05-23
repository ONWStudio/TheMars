using System;
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
    public enum TMBuildingKind : byte
    {
        [InspectorName("개척형")] PIONEER = 0,
        [InspectorName("자원형")] RESOURCEFUL,
        [InspectorName("개발형")] BUILDER,
        [InspectorName("외부형")] EXTERNAL
    }
    
    public sealed class TMBuildingData : ScriptableObject
    {
        [FormerlySerializedAs("effectCreators")]
        [SerializeReference, DisplayAs("건물 효과"), Tooltip("건물 효과 리스트"), SerializeReferenceDropdown]
        private List<ITMBuildingEffectCreator> _effectCreators = new();

        [SerializeField, DisplayAs("건물 시너지")]
        private List<TMSynergyData> _synergies = new();

        [field: SerializeField, ReadOnly] public string ID { get; private set; } = Guid.NewGuid().ToString();        
        
        [field: SerializeField, DisplayAs("건물 키 (건물 키는 고유한 값어야 합니다)")]
        public string BuildingKey { get; private set; } = string.Empty;

        [field: SerializeField, DisplayAs("종류")] public TMBuildingKind Kind { get; private set; } = TMBuildingKind.PIONEER;
        
        [field: SerializeField, DisplayAs("건물 프리팹")] public TMBuilding BuildingPrefab { get; private set; }

        [field: FormerlySerializedAs("_localizedBuildingName")]
        [field: SerializeField, LocalizedString(tableName: "BuildingName"), DisplayAs("건물 이름"), Tooltip("건물 이름은 국가별로 지역화 옵션을 제공합니다")]
        public LocalizedString LocalizedBuildingName { get; private set; }

        public IReadOnlyList<TMSynergyData> Synergies => _synergies;
        
        public IEnumerable<ITMBuildingEffect> CreateBuildingEffects()
        {
            return _effectCreators
                .Select(effectCreator => effectCreator.CreateEffect())
                .ToList();
        }
    }
}
