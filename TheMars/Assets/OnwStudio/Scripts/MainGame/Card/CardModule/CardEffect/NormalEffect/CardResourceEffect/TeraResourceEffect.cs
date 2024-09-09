using Onw.Attribute;
using Onw.Event;
using Onw.Interface;
using Onw.ServiceLocator;
using TM;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect.Resource
{
    public sealed class TeraResourceEffect : ITMCardResourceEffect, ITMCardInitializeEffect<TeraResourceCardEffectCreator>
    {
        public SafeAction<string> Event { get; } = new();

        public string Description => $"<sprite={(int)TMRequiredResource.CREDIT}> {(Amount < 0 ? Amount.ToString() : $"+{Amount}")}";
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        public void Initialize(TeraResourceCardEffectCreator cardEffectCreator)
        {
            Amount = cardEffectCreator.Amount;
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(card =>
            {
                ServiceLocator<PlayerManager>.InvokeService(player => player.Credit += Amount);
                Debug.Log(Amount);
                Debug.Log("테라 획득");
            });
        }

        public void AddRewardResource(int additionalAmount)
        {
            Amount += additionalAmount;
            Event.Invoke("Description Update");
        }
        
        public void Dispose()
        {
        }
    }
}