using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event
{
    public sealed class TMDevelopmentOfAnIllusionEventData : TMEventData
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
