//using System.Collections;
//using System.Collections.Generic;
//using Onw.Attribute;
//using TM.Manager;
//using UnityEngine;

//namespace TM.Event
//{
//    [Substitution("오로라 혁신")]
//    public sealed class TMAuroraInnovationEventData : TMEventData
//    {
//        [field: InfoBox("효과 고유값 Key \n \n" +
//                        "\t 인구 획득량 최소 : PopulationAddByDayMin \n" +
//                        "\t 인구 획득량 최대 : PopulationAddByDayMax \n" +
//                        "\t 인구 획득 지속일수 : PopulationAddDayCount \n" +
//                        "\t 만족도 획득량(하루마다) : SatisfactionAddByDay")]
//        [field: Header("다친 사람들을 치료하고 시민들의 복지에 최선을 다 하자. 선택지")]
//        [field: SerializeField, DisplayAs("인구 획득량 최소"), Min(0)] public int TopPopulationAddByDayMin {get; private set; } = 2;
//        [field: SerializeField, DisplayAs("인구 획득량 최대"), Min(0)] public int TopPopulationAddByDayMax {get; private set; } = 4;
//        [field: SerializeField, DisplayAs("인구 획득 지속일수"), Min(0)] public int TopPopulationAddDayCount {get; private set; } = 7;
//        [field: SerializeField, DisplayAs("만족도 획득량(하루마다)"), Min(0)] public int TopSatisfactionAddByDay {get; private set; } = 10;
        
//        public override bool CanFireTop => true;
//        public override bool CanFireBottom => true;

//        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
//        {
//            { "PopulationAddByDayMin", TopPopulationAddByDayMin },
//            { "PopulationAddByDayMax", TopPopulationAddByDayMax },
//            { "PopulationAddDayCount", TopPopulationAddDayCount },
//            { "SatisfactionAddByDay", TopSatisfactionAddByDay }
//        };
//        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
//        protected override void TriggerTopEvent()
//        {
//            int dayCountByPopulation = 0;

//            TMSimulator.Instance.NowDay.AddListener(onChangedDayByPopulation);
//            TMSimulator.Instance.NowDay.AddListener(onChangedDayBySatisfaction);
            
//            void onChangedDayByPopulation(int day)
//            {
//                dayCountByPopulation++;

//                if (dayCountByPopulation == TopPopulationAddDayCount)
//                {
//                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDayByPopulation);
//                }

//                TMPlayerManager.Instance.SetPopulation(TMPlayerManager.Instance.Population.Value + Random.Range(TopPopulationAddByDayMin, TopPopulationAddByDayMax + 1));
//            }

//            void onChangedDayBySatisfaction(int day)
//            {
//                TMPlayerManager.Instance.Satisfaction.Value += TopSatisfactionAddByDay;
//            }
//        }
        
//        protected override void TriggerBottomEvent()
//        {
//        }
//    }
//}