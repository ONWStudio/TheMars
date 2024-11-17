using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Components;
using Onw.Manager;
using TM.Grid;
using TM.Building;
using UnityEngine;
using UnityEngine.Events;

namespace TM.Synergy
{
    public sealed class TMSynergyManager : SceneSingleton<TMSynergyManager>
    {
        public event UnityAction<IReadOnlyDictionary<string, TMSynergy>> OnUpdateSynergies
        {
            add => _onUpdateSynergies.AddListener(value);
            remove => _onUpdateSynergies.RemoveListener(value);
        }

        private readonly Dictionary<string, TMSynergy> _synergies = new();

        [SerializeField] private UnityEvent<IReadOnlyDictionary<string, TMSynergy>> _onUpdateSynergies = new();

        public IReadOnlyDictionary<string, TMSynergy> Synergies => _synergies;
        protected override string SceneName => "MainGameScene";

        protected override void Init() { }

        public void OnAddedBuilding(TMBuilding building)
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

        public void OnRemovedBuilding(TMBuilding building)
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