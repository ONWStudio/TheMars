using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Manager;
using TM.Building;

namespace TM.Synergy
{
    public sealed class TMSynergyManager : SceneSingleton<TMSynergyManager>
    {
        public event UnityAction<TMSynergy[]> OnUpdateSynergies
        {
            add => _onUpdateSynergies.AddListener(value);
            remove => _onUpdateSynergies.RemoveListener(value);
        }

        private readonly Dictionary<string, TMSynergy> _synergies = new();

        [SerializeField] private UnityEvent<TMSynergy[]> _onUpdateSynergies = new();

        public IReadOnlyDictionary<string, TMSynergy> Synergies => _synergies;
        protected override string SceneName => "MainGameScene";

        protected override void Init() { }

        // TODO : 시너지 레벨 별로 효과를 지정할 수 있게 변경
        public void OnAddedBuilding(TMBuilding building)
        {
            foreach (TMSynergyData synergyData in building.BuildingData.Synergies.Where(synergy => synergy is not null))
            {
                if (!_synergies.TryGetValue(synergyData.ID, out TMSynergy synergy))
                {
                    synergy = synergyData.CreateSynergy();
                    _synergies.Add(synergyData.ID, synergy);
                }

                synergy.BuildingCount++;
                synergy.ApplySynergy();
            }

            _onUpdateSynergies.Invoke(_synergies.Values.ToArray());
        }

        public void OnRemovedBuilding(TMBuilding building)
        {
            foreach (TMSynergyData synergyData in building.BuildingData.Synergies.Where(synergy => synergy is not null))
            {
                if (!_synergies.TryGetValue(synergyData.ID, out TMSynergy synergy)) return;

                synergy.BuildingCount--;
                synergy.ApplySynergy();

                if (synergy.BuildingCount <= 0)
                {
                    _synergies.Remove(synergyData.ID);
                }

                _onUpdateSynergies.Invoke(_synergies.Values.ToArray());
            }
        }
    }
}