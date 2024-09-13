using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Interface;
using Onw.Localization;
using TM.Synergy.Effect;
using UnityEngine.Localization;

namespace TM.Synergy
{
    public sealed class TMSynergyData : ScriptableObject
    {
        public string SynergyName => _localizedSynergyName.TryGetLocalizedString(out string synergyName) ? synergyName : "";
        
        public string[] DescriptionArray => _synergyEffects
            .Select(synergyEffect => synergyEffect.Description)
            .ToArray();
        
        public IReadOnlyList<TMSynergyEffect> SynergyEffects => _synergyEffects;
        
        [field: SerializeField] public int BuildingCount { get; private set; }

        [SerializeReference, SerializeReferenceDropdown] private List<TMSynergyEffect> _synergyEffects = new();

        [SerializeField] private LocalizedString _localizedSynergyName;        
        
        private readonly List<TMSynergyEffect> _applicationEffects = new();
        public int RuntimeBuildingCount { get; set; }

        public void ResetCount()
        {
            RuntimeBuildingCount = BuildingCount;
            _applicationEffects.Clear();
        }

        public void ApplySynergy()
        {
            _applicationEffects.RemoveAll(applicationEffect =>
            {
                if (applicationEffect.TargetBuildingCount > RuntimeBuildingCount)
                {
                    applicationEffect.UnapplyEffect();
                    return true;
                }
                
                return false;
            });
            
            TMSynergyEffect[] shallEffects = _synergyEffects
                .Where(synergyEffect => !_applicationEffects.Contains(synergyEffect) && synergyEffect.TargetBuildingCount <= RuntimeBuildingCount)
                .ToArray();

            shallEffects.ForEach(shallEffect => shallEffect.ApplyEffect());
            _applicationEffects.AddRange(shallEffects);
        }
    }
}
