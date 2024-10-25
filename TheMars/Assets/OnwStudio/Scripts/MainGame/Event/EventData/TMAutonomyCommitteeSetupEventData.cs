using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    public class TMAutonomyCommitteeSetupEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 하루마다 오를 만족도 획득량 : SatisfactionAddByDay \n" +
                        "\t 만족도 획득 지속일수 : SatisfactionAddDayCount")]
        [field: Header("귀찮은 일은 빠르게 처리하자고. 선택지")]
        [field: SerializeField, DisplayAs("하루마다 오를 만족도 획득량"), OnwMin(0)] public int TopSatisfactionAddByDay { get; private set; } = 30;
        [field: SerializeField, DisplayAs("만족도 획득 지속일수"), OnwMin(0)] public int TopSatisfactionAddDayCount { get; private set; } = 3;
        
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionAddByDay", TopSatisfactionAddByDay },
            { "SatisfactionAddDayCount", TopSatisfactionAddDayCount }
        };

        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
        protected override void TriggerTopEvent()
        {
            int dayCount = 0;
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == TopSatisfactionAddDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDay;
                }

                TMPlayerManager.Instance.Satisfaction -= TopSatisfactionAddByDay;
            }
        }

        protected override void TriggerBottomEvent() { }
    }
}
