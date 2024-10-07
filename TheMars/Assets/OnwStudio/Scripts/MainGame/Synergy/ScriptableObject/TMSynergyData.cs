using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using Onw.Localization;
using TM.Synergy.Effect.Creator;

namespace TM.Synergy
{
    public sealed class TMSynergyData : ScriptableObject
    {
        public string SynergyName => _localizedSynergyName.TryGetLocalizedString(out string synergyName) ? synergyName : "";
        
        [field: SerializeField, SpritePreview] public Sprite Icon { get; private set; } 

        [SerializeReference, SerializeReferenceDropdown] private List<TMSynergyEffectCreator> _synergyEffects = new();

        [SerializeField] private LocalizedString _localizedSynergyName;

        public TMSynergy CreateSynergy()
        {
            return new(this, _synergyEffects.Select(effectCreator => effectCreator.CreateEffect()).ToArray());
        }
    }
}
