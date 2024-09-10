using Onw.Attribute;
using Onw.Event;
using Onw.ServiceLocator;
using TM;
using TMCard.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace TMCard.Effect.Resource
{
    public sealed class TeraResourceEffect : ITMCardResourceEffect, ITMCardInitializeEffect<TeraResourceCardEffectCreator>
    {
        public string Description => $"<sprite={(int)TMCostKind.CREDIT}> {(Amount < 0 ? Amount.ToString() : $"+{Amount}")}";
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }
        
        public event UnityAction<string> OnNotifyEvent
        {
            add => _onNotifyEvent.AddListener(value);
            remove => _onNotifyEvent.RemoveListener(value);
        }

        [SerializeField] private UnityEvent<string> _onNotifyEvent = new();

        public void Initialize(TeraResourceCardEffectCreator cardEffectCreator)
        {
            Amount = cardEffectCreator.Amount;
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            trigger.OnEffectEvent += _ =>
            {
                ServiceLocator<PlayerManager>.InvokeService(player => player.Credit += Amount);
                Debug.Log(Amount);
                Debug.Log("테라 획득");
            };
        }

        public void AddRewardResource(int additionalAmount)
        {
            Amount += additionalAmount;
            _onNotifyEvent.Invoke("Description Update");
        }
    }
}