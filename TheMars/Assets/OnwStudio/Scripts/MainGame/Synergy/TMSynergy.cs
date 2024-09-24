using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Extensions;
using TM.Synergy.Effect;
using UnityEngine;

namespace TM.Synergy
{
    public sealed class TMSynergy
    {
        public TMSynergyData SynergyData { get; } = null;

        [field: SerializeField, ReadOnly] public int BuildingCount { get; set; }

        public string[] DescriptionArray => _synergyEffects
            .Select(synergyEffect => synergyEffect.Description)
            .ToArray();
        
        public string SynergyName => SynergyData ? SynergyData.SynergyName : "";
        public IReadOnlyList<TMSynergyEffect> SynergyEffects => _synergyEffects;
        
        private readonly List<TMSynergyEffect> _synergyEffects = new();
        private readonly List<TMSynergyEffect> _applicationEffects = new();
        
        public void ApplySynergy()
        {
            _applicationEffects.RemoveAll(applicationEffect =>
            {
                if (applicationEffect.TargetBuildingCount > BuildingCount)
                {
                    applicationEffect.UnapplyEffect(this);
                    return true;
                }
                
                return false;
            });
            
            TMSynergyEffect[] shallEffects = _synergyEffects
                .Where(synergyEffect => !_applicationEffects.Contains(synergyEffect) && synergyEffect.TargetBuildingCount <= BuildingCount)
                .ToArray();

            shallEffects.ForEach(shallEffect => shallEffect.ApplyEffect(this));
            _applicationEffects.AddRange(shallEffects);
        }
        
        public TMSynergy(TMSynergyData synergyData, TMSynergyEffect[] effects)
        {
            SynergyData = synergyData;
            _synergyEffects.AddRange(effects);
        }
    }
}
