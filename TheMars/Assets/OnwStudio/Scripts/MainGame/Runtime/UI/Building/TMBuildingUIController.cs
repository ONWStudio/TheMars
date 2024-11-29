using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UniRx;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Extensions;
using TM.Building;
using UniRx.Triggers;

namespace TM.UI
{
    public class TMBuildingUIController : MonoBehaviour
    {
        public struct BuildingEffectCallbackFair : IDisposable
        {
            public LocalizedString LocalizedDescription { get; private set; }
            public LocalizedString.ChangeHandler ChangeHandler { get; private set; }

            private bool _disposed;
            
            public void Dispose()
            {
                if (_disposed) return;
                
                LocalizedDescription.StringChanged -= ChangeHandler;
                LocalizedDescription = null;
                _disposed = true;
            }

            public BuildingEffectCallbackFair(LocalizedString localizedDescription, LocalizedString.ChangeHandler changeHandler)
            {
                LocalizedDescription = localizedDescription;
                ChangeHandler = changeHandler;
                LocalizedDescription.StringChanged += changeHandler;
                _disposed = false;
            }
        }
        
        [SerializeField, InitializeRequireComponent] private TMBuildingDescriptor _descriptor;

        private TMBuilding _currentBuilding;
        private readonly Dictionary<int, BuildingEffectCallbackFair[]> _buildingEffectCallbackFairs = new();

        private void Awake()
        {
            this
                .UpdateAsObservable()
                .Select(_ => _descriptor.ActiveDescriptor && EventSystem.current.IsPointerOverGameObject() && _currentBuilding)
                .DistinctUntilChanged()
                .Where(isOver => isOver)
                .Subscribe(_ => ResetDescriptor(_currentBuilding))
                .AddTo(this); 
        }
        
        public void OnEnterBuilding(TMBuilding building)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            _descriptor.ActiveDescriptor = true;
            _currentBuilding = building;
            
            bool isReload = false;
            
            building.BuildingData.LocalizedBuildingName.StringChanged += onChangedName;
            _buildingEffectCallbackFairs.Add(
                building.GetInstanceID(), 
                building
                    .Effects
                    .Select(effect => new BuildingEffectCallbackFair(effect.LocalizedDescription, _ => onChangedEffectDescription()))
                    .ToArray());
            
            // .. 성능 최적화
            void onChangedEffectDescription()
            {
                if (isReload) return;

                isReload = true;
                
                if (_currentBuilding == building)
                {
                    _descriptor.Description = string.Join("\n", _currentBuilding.Effects.Select(effect => effect.LocalizedDescription.GetLocalizedString()));
                }

                this.DoCallWaitForOneFrame(() => isReload = false);
            }
        }

        public void ResetDescriptor(TMBuilding building)
        {
            building.BuildingData.LocalizedBuildingName.StringChanged -= onChangedName;

            if (_buildingEffectCallbackFairs.Remove(building.GetInstanceID(), out BuildingEffectCallbackFair[] fairs))
            {
                fairs.ForEach(fair => fair.Dispose());
            }
            
            if (_currentBuilding == building)
            {
                _descriptor.ActiveDescriptor = false;
                _descriptor.Name = string.Empty;
                _descriptor.Description = string.Empty;
                _currentBuilding = null;
            }
        }

        private void onChangedName(string buildingName)
        {
            _descriptor.Name = buildingName;
        }


    }
}
