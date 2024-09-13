using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.ServiceLocator;
using TM.Grid;
using TM.Building;
using UnityEngine;
using UnityEngine.Events;

namespace TM.Synergy
{
    public sealed class SynergyManager : MonoBehaviour
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

        [SerializeField, ReadOnly] private UnityEvent<IReadOnlyCollection<TMSynergyData>> _onUpdateSynergies = new();
        
        private void Awake()
        {
            if (ServiceLocator<SynergyManager>.RegisterService(this)) return;
            
            ServiceLocator<SynergyManager>.ChangeService(this);
        }

        private IEnumerator Start()
        {
            TMGridManager gridManager = null;
            yield return new WaitUntil(() => ServiceLocator<TMGridManager>.TryGetService(out gridManager));

            gridManager.OnAddedBuilding += onAddedBuilding;
            gridManager.OnRemovedBuilding += onRemovedBuilding;
        }

        private void OnDestroy()
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            gridManager.OnAddedBuilding -= onAddedBuilding;
            gridManager.OnRemovedBuilding -= onRemovedBuilding;
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
