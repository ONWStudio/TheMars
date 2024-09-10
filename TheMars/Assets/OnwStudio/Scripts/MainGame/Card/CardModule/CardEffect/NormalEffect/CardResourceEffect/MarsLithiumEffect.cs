using UnityEngine;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.ServiceLocator;
using TM;
using TMCard.Runtime;

namespace TMCard.Effect.Resource
{
    [System.Serializable]
    public sealed class MarsLithiumEffect : ITMCardResourceEffect, ITMCardInitializeEffect<MarsLithumCardEffectCreator>
    {
        public string Description => $"<sprite={(int)TMCostKind.MARS_LITHIUM}> {(Amount < 0 ? Amount.ToString() : $"+{Amount}")}";

        public event UnityAction<string> OnNotifyEvent
        {
            add => _onNotifyEvent.AddListener(value);
            remove => _onNotifyEvent.RemoveListener(value);
        }

        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        [SerializeField] private UnityEvent<string> _onNotifyEvent = new();

        public void Initialize(MarsLithumCardEffectCreator cardEffectCreator)
        {
            Amount = cardEffectCreator.Amount;
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            trigger.OnEffectEvent += _ =>
            {
                ServiceLocator<PlayerManager>.InvokeService(player => player.MarsLithium += Amount);
                Debug.Log(Amount);
                Debug.Log("마르스 리튬 획득");
            };
        }

        public void AddRewardResource(int additionalAmount)
        {
            Amount += additionalAmount;
            _onNotifyEvent.Invoke(Description);
        }
        
        public void Dispose()
        {
        }
    }
}

