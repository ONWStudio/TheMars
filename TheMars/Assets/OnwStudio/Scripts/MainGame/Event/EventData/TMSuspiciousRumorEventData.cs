using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event
{
    [Substitution("수상한 소문")]
    public sealed class TMSuspiciousRumorEventData : TMEventData
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