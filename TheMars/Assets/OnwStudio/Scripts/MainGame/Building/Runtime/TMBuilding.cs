using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Extensions;
using TM.Building.Effect;

namespace TM.Building
{
    public sealed class TMBuilding : MonoBehaviour
    {
        [field: Header("Building Data")]
        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; }
        
        [field: Header("Components")]
        [field: SerializeField, SelectableSerializeField] public Renderer MeshRenderer { get; private set; }
        
        [field: Header("Building State")]
        [field: SerializeField, ReadOnly] public int BuildingLevel { get; private set; } = 1;
        [field: SerializeField, ReadOnly] public bool OnTile { get; private set; } = false;
        
        [SerializeField, ReadOnly] private List<ITMBuildingEffect> _buildingEffects = new();
        
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