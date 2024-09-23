using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Extensions;
using Onw.Attribute;
using Onw.Interface;
using Onw.Localization;
using TM.Synergy.Effect;
using TM.Synergy.Effect.Creator;
using UnityEngine.Localization;

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
