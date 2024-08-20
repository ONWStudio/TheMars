using Onw.Localization;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    public abstract class TMCardSpecialEffect : ILocalizable, ITMCardEffect
    {
        [field: SerializeField] public LocalizedStringOption StringOption { get; private set; }

        protected TMCardSpecialEffect(string entryReference)
        {
            StringOption = new("CardSpecialEffectName", entryReference);
        }

        public abstract void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger);
    }
}