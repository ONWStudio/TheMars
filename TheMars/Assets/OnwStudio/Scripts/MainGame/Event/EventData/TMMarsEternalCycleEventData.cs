using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event
{
    [Substitution("화성은 늘 자전 한다.")]
    public sealed class TMMarsEternalCycleEventData : TMEventData
    {
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;

        public override Dictionary<string, object> TopEffectLocalizedArguments => null;

        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
        protected override void TriggerTopEvent()
        {
        }
        
        protected override void TriggerBottomEvent()
        {
        }
    }
}
