using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Event.Effect.Creator;
using TM.Manager;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    public sealed class TMEventMarsLithiumEventModifyEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventMarsLithiumEventModifyEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; }
        [field: SerializeField, ReadOnly] public int MarsLithiumAdd { get; private set; }        
        
        public void Initialize(TMEventMarsLithiumEventModifyEffectCreator effectCreator)
        {
            MarsLithiumAdd = effectCreator.MarsLithiumAdd;
        }
        
        public void ApplyEffect()
        {
            int nowDay = TMSimulator.Instance.NowDay.Value;
            
            TMEventManager.Instance.MarsLithiumEventAddResource += MarsLithiumAdd;
            TMEventManager.Instance.OnTriggerEvent += onTriggerEvent;

            void onTriggerEvent(TMEventRunner eventRunner)
            {
                if (eventRunner.EventData != TMEventDataManager.Instance.MarsLithiumEvent || nowDay == TMSimulator.Instance.NowDay.Value) return;

                TMEventManager.Instance.OnTriggerEvent -= onTriggerEvent;
                TMEventManager.Instance.MarsLithiumEventAddResource -= MarsLithiumAdd;
            }
        }

    }
}
