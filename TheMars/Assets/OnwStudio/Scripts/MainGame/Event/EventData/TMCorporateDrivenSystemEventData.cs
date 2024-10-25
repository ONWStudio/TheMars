using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event
{
    public sealed class TMCorporateDrivenSystemEventData : TMEventData
    {
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override List<object> TopEffectLocalizedArguments
        {
            get;
        }
        
        public override List<object> BottomEffectLocalizedArguments
        {
            get;
        }

        protected override void TriggerLeftEvent() { }

        protected override void TriggerRightEvent()
        {
        }
    }
}