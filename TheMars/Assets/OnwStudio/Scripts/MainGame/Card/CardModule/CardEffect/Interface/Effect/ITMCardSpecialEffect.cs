using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Onw.Attribute;
using Onw.Interface;
using Onw.Localization;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public abstract class TMCardSpecialEffect : ILocalizable, ITMCardEffect
    {
        [field: SerializeField] public LocalizedStringOption StringOption { get; private set; }

        public TMCardSpecialEffect(string entryReference)
        {
            StringOption = new("CardSpecialEffectName", entryReference);
        }

        public abstract void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger);
    }
}