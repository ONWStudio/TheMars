using Onw.Attribute;
using System;
using System.Collections;
using System.Collections.Generic;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [Serializable]
    public sealed class TMResourceRepeatAddBuff : TMRepeatBuff, ITMInitializeBuff<TMResourceRepeatAddBuffTrigger>
    {
        [field: SerializeField, ReadOnly] protected override AssetReferenceSprite IconReference { get; set; } = new("Sprites/Smile_Buff_Positive");
        [field: SerializeField, ReadOnly] public TMResourceKind ResourceKind { get; set; }
        [field: SerializeField, ReadOnly] public int Resource { get; set; }

        public override Color IconBackgroundColor => Color.blue;

        public void Initialize(TMResourceRepeatAddBuffTrigger trigger)
        {
            base.Initialize(trigger);

            ResourceKind = trigger.ResourceKind;
            Resource = trigger.Resource;
        }

        protected override void OnChangedDayByRepeatDay(int day)
        {
            TMPlayerManager.Instance.AddResource(ResourceKind, Resource);
        }
    }
}
