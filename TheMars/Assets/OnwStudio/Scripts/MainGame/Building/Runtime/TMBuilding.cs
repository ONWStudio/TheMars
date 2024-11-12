using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Event;
using Onw.Attribute;
using TM.Building.Effect;

namespace TM.Building
{
    public sealed class TMBuilding : MonoBehaviour
    {
        [SerializeReference, SerializeReferenceDropdown, ReadOnly] private List<ITMBuildingEffect> _buildingEffects = new();
        
        [SerializeField, ReadOnly] private ReactiveField<bool> _isActive = new() { Value = true };
        
        [FormerlySerializedAs("_buildingLevel")]
        [Header("Building State")]
        [SerializeField] private ReactiveField<int> _grade  = new() { Value = 1, ValueProcessors = new() { new ClampIntProcessor(1, 3) }};
        [SerializeField] private ReactiveField<bool> _onTile = new();
        
        [field: Header("Building Data")]
        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; }
        
        [field: Header("Components")]
        [field: SerializeField, SelectableSerializeField] public Renderer MeshRenderer { get; private set; }

        public IReactiveField<int> Grade => _grade;
        public IReactiveField<bool> OnTile => _onTile;
        public IReactiveField<bool> IsActive => _isActive;
        
        public TMBuilding Initialize(TMBuildingData buildingData)
        {
            if (BuildingData) return this;

            BuildingData = buildingData;
            _buildingEffects = BuildingData
                .CreateBuildingEffects()
                .ToList();
            
            return this;
        }

        public void BatchOnTile()
        {
            ApplyBuildingEffect();
            MeshRenderer.enabled = true;
        }

        public void ApplyBuildingEffect()
        {
            _buildingEffects
                .ForEach(buildingEffect => buildingEffect.ApplyEffect(this));
        }
        
        public void DisableBuilding()
        {
            _buildingEffects
                .ForEach(effect => effect.DisableEffect(this));
        }
    }
}