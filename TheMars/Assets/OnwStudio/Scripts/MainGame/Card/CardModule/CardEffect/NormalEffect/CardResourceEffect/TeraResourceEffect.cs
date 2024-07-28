using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect.Resource
{
    public sealed class TeraResourceEffect : ITMCardResourceEffect, ITMInitializableEffect<TeraResourceEffectCreator>
    {
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화"), ReadOnly] public int Amount { get; private set; }

        public void ApplyEffect(TMCardController controller)
        {
            OnResourceEffect(controller);
        }

        public void Initialize(TeraResourceEffectCreator effectCreator)
        {
            Amount = effectCreator.Amount;
        }

        public void OnResourceEffect(TMCardController controller, int addtionalAmount = 0)
        {
            Debug.Log(Amount + addtionalAmount);
            Debug.Log("테라 획득");
        }
    }
}