using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Buff;
using TM.Buff.Trigger;
using TM.Event.Effect.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    [System.Serializable]
    public class TMEventBuffEffect : ITMEventEffect, ITMEventInitializeEffect<TMEventBuffEffectCreator>
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; }
        [field: SerializeReference] public TMBuffBase Buff { get; private set; }

        public void ApplyEffect()
        {
            Buff.ApplyBuff();
        }

        public void Initialize(TMEventBuffEffectCreator creator)
        {
            Buff = creator.BuffTrigger.CreateBuff();
        }
    }
}
