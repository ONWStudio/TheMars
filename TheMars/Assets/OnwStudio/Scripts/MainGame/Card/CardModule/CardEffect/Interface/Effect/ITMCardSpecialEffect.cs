using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Localization;
using TMCard.Runtime;

namespace TMCard.Effect
{
    public abstract class TMCardSpecialEffect : ITMCardEffect
    {
        public string Description
        {
            get
            {
                string description = _labelOption.TryGetLocalizedString(out string label) ?
                    $"<color=#DB5231>{label}</color>" :
                    "";

                return this is ITMInnerEffector innerEffector && innerEffector.InnerEffect.Count > 0 ? 
                    "<size=80%>" + string.Join("\n", innerEffector.InnerEffect.Select(effect => $"{description}:{effect.Description}")) + "</size>" : 
                    description;
            }
        }
        
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        [SerializeField] protected LocalizedString _labelOption;

        protected TMCardSpecialEffect(string entryReference)
        {
            _labelOption = new("CardSpecialEffectName", entryReference);
        }

        public abstract void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger);

    }
}