using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [Substitution("성장의 딜레마")]
    public sealed class TMDilemmaOfGrowthEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 하루마다 획득할 만족도량 : SatisfactionAddByDay \n" +
                        "\t 만족도 획득 지속일수 : SatisfactionAddDayCount \n" +
                        "\t 등급 하락시킬 건물 개수 : TargetBuildingCount")]
        [field: Header("지금이라도 그들을 제지 해야겠어. 선택지")]
        [field: SerializeField, DisplayAs("하루마다 획득할 만족도량"), OnwMin(0)] public int TopSatisfactionAddByDay { get; private set; } = 5;
        [field: SerializeField, DisplayAs("만족도 획득 지속일수"), OnwMin(0)] public int TopSatisfactionAddDayCount { get; private set; } = 2;
        [field: SerializeField, DisplayAs("등급 하락시킬 건물 개수"), OnwMin(0)] public int TopTargetBuildingCount { get; private set; } = 2;

        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 하루마다 감소시킬 인구 수 최소값 : PopulationSubtractByDayMin \n" +
                        "\t 하루마다 감소시킬 인구 수 최대값 : PopulationSubtractByDayMax \n" +
                        "\t 인구 수 감소 지속일수 : PopulationSubtractDayCount \n")]
        [field: Header("저희가 친환경 기술을 개발 하겠습니다. 선택지")]
        [field: SerializeField, DisplayAs("하루마다 감소시킬 인구 수 최소값"), OnwMin(0)] public int BottomPopulationSubtractByDayMin { get; private set; } = 2;
        [field: SerializeField, DisplayAs("하루마다 감소시킬 인구 수 최대값"), OnwMin(0)] public int BottomPopulationSubtractByDayMax { get; private set; } = 4;
        [field: SerializeField, DisplayAs("인구 수 감소 지속일수"), OnwMin(0)] public int BottomPopulationSubtractDayCount { get; private set; } = 4;
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionAddByDay", TopSatisfactionAddByDay },
            { "SatisfactionAddDayCount", TopSatisfactionAddDayCount },
            { "TargetBuildingCount", TopTargetBuildingCount }
        };

        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
        {
            { "PopulationSubtractByDayMin", BottomPopulationSubtractByDayMin },
            { "PopulationSubtractByDayMax", BottomPopulationSubtractByDayMax },
            { "PopulationSubtractDayCount", BottomPopulationSubtractDayCount }
        };
        
        protected override void TriggerTopEvent()
        {
            int dayCount = 0;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            
            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == TopSatisfactionAddDayCount)
                {
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                }

                TMPlayerManager.Instance.Satisfaction.Value -= TopSatisfactionAddByDay;
            }
        }
        
        protected override void TriggerBottomEvent()
        {
            if (BottomPopulationSubtractByDayMin > BottomPopulationSubtractByDayMax)
            {
                Debug.LogWarning("하루마다 감소시킬 인구 수의 범위가 올바르지 않습니다 최대값보다 최소값이 더 큽니다");
                return;
            }

            int dayCount = 0;
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == BottomPopulationSubtractDayCount)
                {
                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
                }

                TMPlayerManager.Instance.SetPopulation(TMPlayerManager.Instance.Population.Value - Random.Range(BottomPopulationSubtractByDayMin, BottomPopulationSubtractByDayMax + 1));
            }
        }
    }
}