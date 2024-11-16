using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;
using Onw.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TM.Building
{
    public sealed class TMBuildingDataManager : ScriptableObjectSingleton<TMBuildingDataManager>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/ScriptableObjectManager/Resources/TMBuildingDataManager.asset";

        public IReadOnlyList<TMBuildingData> BuildingDataList => _buildingDataList;
        
        [SerializeField, ReadOnly] private List<TMBuildingData> _buildingDataList = new();

        #if UNITY_EDITOR
        internal void AddBuilding(TMBuildingData buildingData)
        {
            if (_buildingDataList.Contains(buildingData)) return;
            
            _buildingDataList.Add(buildingData);
            EditorUtility.SetDirty(this);
        }

        internal bool RemoveBuilding(TMBuildingData buildingData)
        {
            bool isRemove = _buildingDataList.Remove(buildingData);
            EditorUtility.SetDirty(this);
            
            return isRemove;
        }
        #endif

        public TMBuildingData[] GetBuildingDataArrayByWhere(Func<TMBuildingData, bool> predicate)
        {
            return _buildingDataList
                .Where(predicate)
                .ToArray();
        }
    }
}
