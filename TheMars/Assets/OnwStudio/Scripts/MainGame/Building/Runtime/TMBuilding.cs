using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Extensions;
using Onw.Attribute;
using TM.Building.Effect;

namespace TM.Building
{
    public sealed class TMBuilding : MonoBehaviour
    {
        [field: Header("Building State")]
        [field: SerializeField, ReadOnly] public int BuildingLevel { get; private set; } = 1;
        [field: SerializeField, ReadOnly] public bool OnTile { get; private set; } = false;
        
        [Header("Building Data")]
        [SerializeField, ReadOnly] private TMBuildingData _buildingData;

        [SerializeField, ReadOnly] private List<ITMBuildingEffect> _buildingEffects;
        
        public void Initialize(TMBuildingData buildingData)
        {
            if (_buildingData) return;

            _buildingData = buildingData;
            _buildingEffects = _buildingData.BuildingEffects;
        }

        public void BatchOnTile()
        {
            ApplyBuildingEffect();
        }

        public void ApplyBuildingEffect()
        {
            foreach (ITMBuildingEffect buildingEffect in _buildingEffects)
            {
                switch (BuildingLevel)
                {
                    case 1:
                        buildingEffect.ApplyEffectLevelOne(this);
                        break;
                    case 2:
                        buildingEffect.ApplyEffectLevelTwo(this);
                        break;
                    case 3:
                        buildingEffect.ApplyEffectLevelThree(this);
                        break;
                }
            }
        }
        
        public void DisableBuilding()
        {
            _buildingData.BuildingEffects.ForEach(effect => effect.DisableEffect(this));
        }
    }
}