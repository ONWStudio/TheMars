using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Interface;
using Onw.ServiceLocator;
using TM.Grid;
using TM.Building;
using UnityEngine;
using UnityEngine.Events;

namespace TM.Synergy
{
    public sealed class SynergyManager : MonoBehaviour
    {
        public event UnityAction<IReadOnlyList<TMSynergy>> OnUpdateSynergies
        {
            add => _onUpdateSynergies.AddListener(value);
            remove => _onUpdateSynergies.RemoveListener(value);
        }

        public IReadOnlyDictionary<TMCorporation, TMSynergy> Synergies => _synergies;
        
        private readonly Dictionary<TMCorporation, TMSynergy> _synergies = new();

        [SerializeField, ReadOnly] private UnityEvent<IReadOnlyList<TMSynergy>> _onUpdateSynergies = new();
        
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
            _onUpdateSynergies.Invoke();
        }

        private void onRemovedBuilding(TMBuilding building)
        {
            
        }
    }
}
