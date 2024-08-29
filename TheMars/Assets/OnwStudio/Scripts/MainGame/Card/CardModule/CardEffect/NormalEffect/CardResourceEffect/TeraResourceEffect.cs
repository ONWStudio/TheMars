using Onw.Attribute;
using Onw.Event;
using Onw.Interface;
using TM;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect.Resource
{
    public sealed class TeraResourceEffect : ITMCardResourceEffect, ITMCardInitializeEffect<TeraResourceCardEffectCreator>
    {
        public SafeAction<string> Event { get; } = new();

        public string Description => $"<sprite={(int)TMRequiredResource.TERA}> {(Amount < 0 ? Amount.ToString() : $"+{Amount}")}";
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        public void Initialize(TeraResourceCardEffectCreator cardEffectCreator)
        {
            Amount = cardEffectCreator.Amount;
        }

        public void ApplyEffect(TMCardModel model, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(eventState =>
            {
                PlayerManager.Instance.Tera += Amount;
                Debug.Log(Amount);
                Debug.Log("테라 획득");
            });
        }

        public void AddRewardResource(int addtionalAmount)
        {
            Amount += addtionalAmount;
            Event.Invoke("Description Update");
        }
    }
}