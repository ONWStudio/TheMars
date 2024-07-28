using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;
using Onw.Attribute;

namespace TMCard.Effect.Resource
{
    public sealed class MarsLithumEffect : ITMCardResourceEffect, ITMInitializableEffect<MarsLithumEffectCreator>
    {
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        public void ApplyEffect(TMCardController controller)
        {
            OnResourceEffect(controller);
        }

        public void Initialize(MarsLithumEffectCreator effectCreator)
        {
            Amount = effectCreator.Amount;
        }

        public void OnResourceEffect(TMCardController controller, int addtionalAmount = 0)
        {
            Debug.Log(Amount + addtionalAmount);
            Debug.Log("마르스 리튬 획득");
        }
    }
}

