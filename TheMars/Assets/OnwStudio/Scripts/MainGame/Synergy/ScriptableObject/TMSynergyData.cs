using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using Onw.Attribute;
using Onw.Localization;
using TM.Synergy.Effect.Creator;

namespace TM.Synergy
{
    public sealed class TMSynergyData : ScriptableObject
    {
        [field: SerializeField, ReadOnly] public string ID { get; private set; } = Guid.NewGuid().ToString();
        [field: SerializeField, SpritePreview] public Sprite Icon { get; private set; } 

        [SerializeReference, SerializeReferenceDropdown] private List<TMSynergyEffectCreator> _synergyEffects = new();

        [field: SerializeField, LocalizedString(tableName: "SynergyName")] 
        public LocalizedString LocalizedSynergyName { get; private set; }

        public TMSynergy CreateSynergy()
        {
            return new(this, _synergyEffects.Select(effectCreator => effectCreator.CreateEffect()).ToArray());
        }
    }
}
