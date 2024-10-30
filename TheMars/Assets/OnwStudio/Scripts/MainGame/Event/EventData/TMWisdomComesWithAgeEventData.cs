using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [Substitution("사람이 오래면 지혜요. 물건이 오래면 귀신이다. ")]
    public sealed class TMWisdomComesWithAgeEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 만족도 획득량 : SatisfactionAddByDay \n" +
                        "\t 인구 획득량 : PopulationAddByDay \n" +
                        "\t 인구 획득일 간격 : PopulationDayCount \n" +
                        "\t 긍정 이벤트 확률 증가값 : PositiveEventProbabilityAdd \n")]
        [field: Header("")]
        [field: SerializeField, DisplayAs(""), OnwMin(0)] public int TopSatisfactionAddByDay { get; private set; } = 20;
        [field: SerializeField, DisplayAs(""), OnwMin(0)] public int TopPopulationAddByDay { get; private set; } = 4;
        [field: SerializeField, DisplayAs(""), OnwMin(0)] public int TopPopulationAddDayCount { get; private set; } = 4;
        [field: SerializeField, DisplayAs(""), OnwMin(0)] public int TopPositiveEventProbabilityAdd { get; private set; } = 10;

        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionAddByDay", TopSatisfactionAddByDay },
            { "PopulationAddByDay", TopPopulationAddByDay },
            { "PopulationAddDayCount", TopPopulationAddDayCount },
            { "PositiveEventProbabilityAdd", TopPositiveEventProbabilityAdd }
        };

        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        protected override void TriggerTopEvent()
        {
            int dayCountByPopulation = 0;
            
            TMSimulator.Instance.OnChangedDay += onChangedDayBySatisfaction;
            TMSimulator.Instance.OnChangedDay += onChangedDayByPopulation;
            
            void onChangedDayBySatisfaction(int day)
            {
                TMPlayerManager.Instance.Satisfaction += TopSatisfactionAddByDay;
            }

            void onChangedDayByPopulation(int day)
            {
                dayCountByPopulation++;

                if (dayCountByPopulation % TopPopulationAddDayCount == 0)
                {
                    TMPlayerManager.Instance.Population += TopPopulationAddByDay;
                }
            }
        }
        
        protected override void TriggerBottomEvent()
        {
        }
    }
}