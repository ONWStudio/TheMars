using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using TM.Event.Effect.Creator;
using TM.Buff.Trigger;
using TM.Event;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMEventAdditionalProbabilityBuff : TMDelayBuff, ITMInitializeBuff<TMEventAdditionalProbabilityBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Event_Additional_Probability_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField] public TMEventKind Kind { get; private set; }
        [field: SerializeField] public int AddProbability { get; private set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMEventAdditionalProbabilityBuffTrigger creator)
        {
            Kind = creator.Kind;
            AddProbability = creator.AddProbabiltiy;
            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    Kind,
                    Probability = Mathf.Abs(AddProbability),
                    Positive = AddProbability >= 0,
                }
            };
        }

        protected override void OnApplyBuff()
        {
            switch (Kind)
            {
                case TMEventKind.CALAMITY:
                    TMEventManager.Instance.CalamityEventProbability.AdditionalProbability.Value += AddProbability; 
                    break;
                case TMEventKind.POSITIVE:
                    TMEventManager.Instance.PositiveEventProbability.AdditionalProbability.Value += AddProbability;
                    break;
                case TMEventKind.NEGATIVE:
                    TMEventManager.Instance.NegativeEventProbability.AdditionalProbability.Value += AddProbability;
                    break;
            }
        }

        protected override void OnChangedDayByDelayCount(int day)
        {
            switch (Kind)
            {
                case TMEventKind.CALAMITY:
                    TMEventManager.Instance.CalamityEventProbability.AdditionalProbability.Value -= AddProbability;
                    break;
                case TMEventKind.POSITIVE:
                    TMEventManager.Instance.PositiveEventProbability.AdditionalProbability.Value -= AddProbability;
                    break;
                case TMEventKind.NEGATIVE:
                    TMEventManager.Instance.NegativeEventProbability.AdditionalProbability.Value -= AddProbability;
                    break;
            }
        }
    }
}
