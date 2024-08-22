using Onw.Attribute;
using Onw.Event;
using TM;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect.Resource
{
    public sealed class MarsLithumEffect : ITMCardResourceEffect, ITMInitializeEffect<MarsLithumEffectCreator>
    {
        public string Description => $"<sprite={(int)TMRequiredResource.MARS_LITHIUM}> {(Amount < 0 ? Amount.ToString() : $"+{Amount}")}";

        public SafeAction<string> Event { get; } = new();

        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        public void Initialize(MarsLithumEffectCreator effectCreator)
        {
            Amount = effectCreator.Amount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState =>
            {
                PlayerManager.Instance.MarsLithum += Amount;
                Debug.Log(Amount);
                Debug.Log("마르스 리튬 획득");
            });
        }

        public void AddRewardResource(int addtionalAmount)
        {
            Amount += addtionalAmount;
            Event.Invoke("Description Update");
        }
    }
}

