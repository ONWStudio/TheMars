using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    public class TMMomentOfACrackEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 key \n \n" +
                        "\t 무작위 인구 감소 최소값 : PopulationSubtractMin \n" +
                        "\t 무작위 인구 감소 최대값 : PopulationSubtractMax \n" +
                        "\t 만족도 감소량 : SatisfactionSubtractByDay \n" +
                        "\t 만족도 감소 지속일수 : SatisfactionSubtractDayCount")]
        [field: Header("기술을 지켜! 선택지")]
        [field: SerializeField, DisplayAs("무작위 인구 감소 최소값"), OnwMin(0)] public int TopPopulationSubtractMin { get; private set; } = 5;
        [field: SerializeField, DisplayAs("무작위 인구 감소 최대값"), OnwMin(0)] public int TopPopulationSubtractMax { get; private set; } = 10;
        [field: SerializeField, DisplayAs("만족도 감소량"), OnwMin(0)] public int TopSatisfactionSubtractByDay { get; private set; } = 5;
        [field: SerializeField, DisplayAs("만족도 감소 지속일수"), OnwMin(0)] public int TopSatisfactionSubtractDayCount { get; private set; } = 5;

        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 만족도 증가량 : SatisfactionAddByDay \n" +
                        "\t 만족도 증가 지속일수 : SatisfactionDayCount")]
        [field: Header("사람들을 대피 시켜! 선택지")]
        [field: SerializeField, DisplayAs("만족도 증가량"), OnwMin(0)] public int BottomSatisfactionAddByDay {get; private set; } = 5;
        [field: SerializeField, DisplayAs("만족도 증가 지속일수"), OnwMin(0)] public int BottomSatisfactionAddDayCount {get; private set; } = 5;
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "PopulationSubtractMin", TopPopulationSubtractMin },
            { "PopulationSubtractMax", TopPopulationSubtractMax },
            { "SatisfactionSubtractByDay", TopSatisfactionSubtractByDay },
            { "SatisfactionSubtractDayCount", TopSatisfactionSubtractDayCount }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
        {
            { "SatisfactionAddByDay", BottomSatisfactionAddByDay },
            { "SatisfactionAddDayCount", BottomSatisfactionAddDayCount }
        };
        
        protected override void TriggerTopEvent()
        {
            if (TopPopulationSubtractMin <= TopPopulationSubtractMax)
            {
                TMPlayerManager.Instance.Population -= Random.Range(TopPopulationSubtractMin, TopPopulationSubtractMax + 1);
            }
            else
            {
                Debug.LogWarning("무작위 인구 증가값의 최소값이 최대값보다 큽니다!");
            }

            int dayCount = 0;
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == TopSatisfactionSubtractDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDay;
                }
                
                TMPlayerManager.Instance.Population -= TopSatisfactionSubtractByDay;
            }
        }
        
        protected override void TriggerBottomEvent()
        {
            int dayCount = 0;
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == BottomSatisfactionAddDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDay;
                }

                TMPlayerManager.Instance.Population += BottomSatisfactionAddByDay;
            }
        }
    }
}
