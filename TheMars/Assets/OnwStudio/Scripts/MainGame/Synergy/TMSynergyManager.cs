using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Grid;
using TM.Building;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace TM.Synergy
{
    public sealed class TMSynergyManager : MonoBehaviour
    {
        public readonly struct SynergyArgs
        {
            
        }
        
        public event UnityAction<IReadOnlyCollection<TMSynergyData>> OnUpdateSynergies
        {
            add => _onUpdateSynergies.AddListener(value);
            remove => _onUpdateSynergies.RemoveListener(value);
        }

        private IReadOnlyCollection<TMSynergyData> Synergies => _synergies;
        private readonly HashSet<TMSynergyData> _synergies = new();
        [SerializeField, ReadOnly, Inject] private TMGridManager _gridManager; 

        [SerializeField, ReadOnly] private UnityEvent<IReadOnlyCollection<TMSynergyData>> _onUpdateSynergies = new();
        
        private void Start()
        {
            _gridManager.OnAddedBuilding += onAddedBuilding;
            _gridManager.OnRemovedBuilding += onRemovedBuilding;
        }

        private void OnDestroy()
        {
            _gridManager.OnAddedBuilding -= onAddedBuilding;
            _gridManager.OnRemovedBuilding -= onRemovedBuilding;
        }

        private void onAddedBuilding(TMBuilding building)
        {
            foreach (TMSynergyData synergy in building.BuildingData.Synergies)
            {
                if (!_synergies.Contains(synergy))
                {
                    synergy.ResetCount();
                    _synergies.Add(synergy);
                }
                
                synergy.RuntimeBuildingCount++;
                synergy.ApplySynergy();
            }
            
            _onUpdateSynergies.Invoke(_synergies);
        }

        private void onRemovedBuilding(TMBuilding building)
        {
            foreach (TMSynergyData synergy in building.BuildingData.Synergies)
            {
                synergy.RuntimeBuildingCount--;

                if (synergy.RuntimeBuildingCount <= 0)
                {
                    synergy.ResetCount();
                    _synergies.Remove(synergy);
                }
                
                synergy.ApplySynergy();
                _onUpdateSynergies.Invoke(_synergies);
            }
        }
    }
}
