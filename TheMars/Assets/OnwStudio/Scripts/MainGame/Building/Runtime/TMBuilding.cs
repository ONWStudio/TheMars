using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Extensions;
using Onw.Attribute;
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
        
        public void Initialize(TMBuildingData buildingData)
        {
            if (BuildingData) return;

            BuildingData = buildingData;
            _buildingEffects = BuildingData
                .CreateBuildingEffects()
                .ToList();
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