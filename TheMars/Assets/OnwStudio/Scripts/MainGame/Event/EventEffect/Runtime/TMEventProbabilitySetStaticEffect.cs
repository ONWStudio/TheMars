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
                    Kind,
                    Probability
                }
            };
        }

        public void ApplyEffect()
        {
            switch (Kind)
            {
                case TMEventKind.CALAMITY:
                    setCurrentEventProbability(TMEventManager.Instance.CalamityEventProbability);
                    break;
                case TMEventKind.POSITIVE:
                    setCurrentEventProbability(TMEventManager.Instance.PositiveEventProbability);
                    break;
                case TMEventKind.NEGATIVE:
                    setCurrentEventProbability(TMEventManager.Instance.NegativeEventProbability);
                    break;
            }

            void setCurrentEventProbability(ITMEventProbability eventProbability)
            {
                eventProbability.IsStatic.Value = true;
                eventProbability.DefaultProbability.Value = Probability;
            }
        }
    }
}
