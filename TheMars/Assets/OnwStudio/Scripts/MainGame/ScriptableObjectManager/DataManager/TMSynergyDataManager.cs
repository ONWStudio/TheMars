using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Manager;

#if UNITY_EDITOR
using UnityEditor;
// ReSharper disable UnusedMember.Local
#endif

namespace TM.Synergy
{
    public sealed class TMSynergyDataManager : ScriptableObjectSingleton<TMSynergyDataManager>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/ScriptableObjectManager/Resources/TMSynergyDataManager.asset";

        public IReadOnlyList<TMSynergyData> SynergyDataList => _synergyDataList;
        
        [SerializeField] private List<TMSynergyData> _synergyDataList = new();

        #if UNITY_EDITOR
        internal void AddSynergyData(TMSynergyData synergyData)
        {
            if (_synergyDataList.Contains(synergyData)) return;
            
            _synergyDataList.Add(synergyData);
            EditorUtility.SetDirty(this);
        }

        internal bool RemoveSynergyData(TMSynergyData synergyData)
        {
            bool isRemove = _synergyDataList.Remove(synergyData);
            EditorUtility.SetDirty(this);
            
            return isRemove;
        }
        #endif

        public TMSynergyData[] GetSynergyDataArrayByWhere(Func<TMSynergyData, bool> predicate)
        {
            return _synergyDataList
                .Where(predicate)
                .ToArray();
        }
    }
}
