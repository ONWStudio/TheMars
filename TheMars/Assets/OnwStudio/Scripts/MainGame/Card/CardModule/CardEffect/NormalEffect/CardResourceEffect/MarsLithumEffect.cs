using Onw.Attribute;
using TM;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect.Resource
{
    public sealed class MarsLithumEffect : ITMCardResourceEffect, ITMInitializableEffect<MarsLithumEffectCreator>
    {
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        public void Initialize(MarsLithumEffectCreator effectCreator)
        {
            Amount = effectCreator.Amount;
        }

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            trigger.OnEffectEvent.AddListener(() =>
            {
                PlayerManager.Instance.MarsLithum += Amount;
                Debug.Log(Amount);
                Debug.Log("마르스 리튬 획득");
            });
        }

        public void AddRewardResource(int addtionalAmount)
        {
            Amount += addtionalAmount;
        }
    }
}

