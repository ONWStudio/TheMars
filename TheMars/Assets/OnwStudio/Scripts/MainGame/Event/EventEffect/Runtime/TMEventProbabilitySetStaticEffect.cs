using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Event.Effect.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventProbabilitySetStaticEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventProbabilitySetStaticEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; } = new("TM_Event_Effect", "Probability_Set_Static_Effect");
        [field: SerializeField] public TMEventKind Kind { get; private set; }
        [field: SerializeField] public int Probability { get; private set; }
        
        public void Initialize(TMEventProbabilitySetStaticEffectCreator creator)
        {
            Kind = creator.Kind;
            Probability = creator.Probability;

            EffectDescription.Arguments = new object[]
            {
                new
                {
                    Kind = Kind.ToString(),
                    Probability
                }
            };
        }

        public void ApplyEffect()
        {
            switch (Kind)
            {
                case TMEventKind.CALAMITY:
                    TMEventManager.Instance.CalamityEventProbability.IsStatic.Value = true;
                    TMEventManager.Instance.CalamityEventProbability.DefaultProbability.Value = Probability;
                    break;
                case TMEventKind.POSITIVE:
                    TMEventManager.Instance.PositiveEventProbability.IsStatic.Value = true;
                    TMEventManager.Instance.PositiveEventProbability.DefaultProbability.Value = Probability;
                    break;
                case TMEventKind.NEGATIVE:
                    TMEventManager.Instance.NegativeEventProbability.IsStatic.Value = true;
                    TMEventManager.Instance.NegativeEventProbability.DefaultProbability.Value = Probability;
                    break;
            }
        }
    }
}
