using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    public sealed class TMPriceOfGrowthEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 등급 하락시킬 건물 개수 : TargetBuildingCount \n" + 
                        "\t 등급 하락 지속일수 : DeclineGradeDayCount \n" +
                        "\t 하루마다 감소시킬 만족도량 : SatisfactionSubtractByDay \n" +
                        "\t 만족도 감소 지속일수 : SatisfactionSubtractDayCount")]
        [field: Header("새로운 도전을 해야 된다. 길을 개척해보자. 선택지")]
        [field: SerializeField, DisplayAs("만족도 감소량"), OnwMin(0)] public int TopSatisfactionSubtract { get; private set; } = 20;
        [field: SerializeField, DisplayAs("인구 감소량"), OnwMin(0)] public int TopPopulationSubtract { get; private set; } = 5;
        [field: SerializeField, DisplayAs("등급 하락시킬 건물 개수"), OnwMin(0)] public int TopTargetBuildingCount { get; private set; } = 2;
        [field: SerializeField, DisplayAs("등급 하락 지속일수"), OnwMin(0)] public int TopDeclineGradeDayCount { get; private set; } = 2;
        [field: SerializeField, DisplayAs("하루마다 감소시킬 만족도량"), OnwMin(0)] public int TopSatisfactionSubtractByDay { get; private set; } = 10;
        [field: SerializeField, DisplayAs("만족도 감소 지속일수"), OnwMin(0)] public int TopSatisfactionSubtractDayCount { get; private set; } = 2;
        
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 하루마다 감소시킬 만족도량 : SatisfactionSubtractByDay \n" +
                        "\t 만족도 감소 지속일수 : SatisfactionSubtractDayCount")]        
        [field: Header("이번에 새로운 실험을 한다는데 그쪽에 투자해보자. 선택지")]
        [field: SerializeField, DisplayAs("하루마다 감소시킬 만족도량"), OnwMin(0)] public int BottomSatisfactionSubtractByDay { get; private set; } = 10;
        [field: SerializeField, DisplayAs("만족도 감소 지속일수"), OnwMin(0)] public int BottomSatisfactionSubtractDayCount { get; private set; } = 2;

        public override bool CanFireTop => TMPlayerManager.Instance.Population >= TopPopulationSubtract;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionSubtract", TopSatisfactionSubtract }, 
            { "PopulationSubtract", TopPopulationSubtract },
            { "TargetBuildingCount", TopTargetBuildingCount },
            { "DeclineGradeDayCount", TopDeclineGradeDayCount },
            { "SatisfactionSubtractByDay", TopSatisfactionSubtractByDay },
            { "SatisfactionSubtractDayCount", TopSatisfactionSubtractDayCount }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
        {
            { "SatisfactionSubtractByDay", BottomSatisfactionSubtractByDay },
            { "SatisfactionSubtractDayCount", BottomSatisfactionSubtractDayCount }
        };
        
        protected override void TriggerTopEvent()
        {
            TMPlayerManager.Instance.Satisfaction -= TopSatisfactionSubtract;
            TMPlayerManager.Instance.Population -= TopPopulationSubtract;

            int dayCount = 0;
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == TopSatisfactionSubtractDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDay;
                }

                TMPlayerManager.Instance.Satisfaction -= TopSatisfactionSubtractByDay;
            }
        }
        protected override void TriggerBottomEvent()
        {
            int dayCount = 0;
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == BottomSatisfactionSubtractDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDay;
                }

                TMPlayerManager.Instance.Satisfaction -= BottomSatisfactionSubtractByDay;
            }
        }
    }
}