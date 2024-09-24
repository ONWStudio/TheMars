using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Manager;
using TM.Grid;
using TM.Building;
using UnityEngine;
using UnityEngine.Events;

namespace TM.Synergy
{
    public sealed class TMSynergyManager : SceneSingleton<TMSynergyManager>
    {
        public override string SceneName => "MainGameScene";
        
        public event UnityAction<IReadOnlyDictionary<string, TMSynergy>> OnUpdateSynergies
        {
            add => _onUpdateSynergies.AddListener(value);
            remove => _onUpdateSynergies.RemoveListener(value);
        }

        private IReadOnlyDictionary<string, TMSynergy> Synergies => _synergies;
        private readonly Dictionary<string, TMSynergy> _synergies = new();

        [SerializeField, ReadOnly] private UnityEvent<IReadOnlyDictionary<string, TMSynergy>> _onUpdateSynergies = new();
        
        private void Start()
        {
            TMGridManager.Instance.OnAddedBuilding += onAddedBuilding;
            TMGridManager.Instance.OnRemovedBuilding += onRemovedBuilding;
        }

        protected override void Init() {}

        private void OnDestroy()
        {
            TMGridManager.Instance.OnAddedBuilding -= onAddedBuilding;
            TMGridManager.Instance.OnRemovedBuilding -= onRemovedBuilding;
        }

        private void onAddedBuilding(TMBuilding building)
        {
            foreach (TMSynergyData synergyData in building.BuildingData.Synergies)
            {
                if (!_synergies.TryGetValue(synergyData.SynergyName, out TMSynergy synergy))
                {
                    synergy = synergyData.CreateSynergy();
                    _synergies.Add(synergyData.SynergyName, synergy);
                }
                
                synergy.BuildingCount++;
                synergy.ApplySynergy();
            }
            
            _onUpdateSynergies.Invoke(_synergies);
        }

        private void onRemovedBuilding(TMBuilding building)
        {
            foreach (TMSynergyData synergyData in building.BuildingData.Synergies)
            {
                if (!_synergies.TryGetValue(synergyData.SynergyName, out TMSynergy synergy)) return;
                
                synergy.BuildingCount--;
                synergy.ApplySynergy();

                if (synergy.BuildingCount <= 0)
                {
                    _synergies.Remove(synergy.SynergyName);
                }
                
                _onUpdateSynergies.Invoke(_synergies);
            }
        }
    }
}
