using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Attribute;
using Onw.Interface;
using TM.Synergy.Effect;

namespace TM.Synergy
{
    public class TMSynergy : ScriptableObject
    {
        public readonly struct SynergyEffectDescriptionPair
        {
            public int Index { get; }
            public string Description { get; }

            public SynergyEffectDescriptionPair(int index, string description)
            {
                Index = index;
                Description = description;
            }
        }

        public SynergyEffectDescriptionPair[] DescriptionArray => _synergyEffects
            .Select((effect, i) => new SynergyEffectDescriptionPair(i, effect.Description))
            .ToArray();
        
        public IReadOnlyList<TMSynergyEffect> SynergyEffects => _synergyEffects;
        
        [field: SerializeField] public int BuildingCount { get; private set; }
        [field: SerializeField, ReadOnly] public int RuntimeBuildingCount { get; set; }

        [SerializeReference, SerializeReferenceDropdown] private List<TMSynergyEffect> _synergyEffects = new();

        public void ResetCount()
        {
            RuntimeBuildingCount = BuildingCount;
        }

        public void ApplySynergy()
        {
            
        }
    }
}
