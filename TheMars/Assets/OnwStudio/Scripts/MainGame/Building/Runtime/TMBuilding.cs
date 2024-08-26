using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Onw.Attribute;
using TM.Building.Effect;
using UnityEngine;

namespace TM.Building
{
    public sealed class TMBuilding : MonoBehaviour
    {
        [field: SerializeField] public int BuildingLevel { get; private set; } = 1;

        [SerializeField] private List<ITMBuildingEffect> _buildingEffects = new();

        [SerializeField, ReadOnly] private TMBuildingData _buildingData;


        public void Initialize(TMBuildingData buildingData)
        {
            if (_buildingData) return;

            _buildingData = buildingData;
            _buildingEffects = _buildingData.BuildingEffects;
        }

        public void BatchOnTile()
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
                    default:
                        buildingEffect.ApplyEffectLevelOne(this);
                        break;
                }
            }
        }

        public void DisableBuilding()
        {
            _buildingEffects.ForEach(effect => effect.DisableEffect(this));
        }
    }
}