using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Localization;
using TM.Building.Effect;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using TM.Building.Effect.Creator;

namespace TM.Building
{
    public sealed class TMBuildingData : ScriptableObject
    {
        public string CardName => _localizedCardName.TryGetLocalizedString(out string buildingName) ? buildingName : "";
        
        public List<ITMBuildingEffect> BuildingEffects => _effectCreators
            .Select(effectCreator => effectCreator.CreateEffect())
            .ToList();
        
        [SerializeField] private LocalizedString _localizedCardName;
        
        [FormerlySerializedAs("effectCreators")]
        [SerializeReference, DisplayAs("건물 효과"), Tooltip("건물 효과 리스트"), SerializeReferenceDropdown]
        private List<ITMBuildingEffectCreator> _effectCreators = new();
    }
}
