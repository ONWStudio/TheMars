using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [Substitution("초록빛 희망")]
    public sealed class TMGreenHopeEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "만족도 감소량(하루마다) : SatisfactionSubtractByDay")]
        [field: Header("선택지 1")]
        [field: SerializeField, DisplayAs(""), OnwMin(0)] public int TopSatisfactionSubtractByDay { get; private set; } = 5;

        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionSubtractByDay", TopSatisfactionSubtractByDay }
        };

        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
        protected override void TriggerTopEvent()
        {
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                TMPlayerManager.Instance.Satisfaction -= TopSatisfactionSubtractByDay;
            }
        }
        
        protected override void TriggerBottomEvent()
        {
        }
    }
}
