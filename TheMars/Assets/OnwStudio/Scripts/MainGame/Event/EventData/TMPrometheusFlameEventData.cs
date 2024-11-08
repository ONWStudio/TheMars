using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [Substitution("프로메테우스의 불꽃")]
    public sealed class TMPrometheusFlameEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 key \n \n" +
                        "\t 식물 감소량(하루마다) : PlantsSubtractByDay \n" +
                        "\t 재난 확률 고정 수치 : FixationDisasterProbability")]
        [field: Header("선택지 1")]
        [field: SerializeField, DisplayAs("식물 감소량(하루마다)"), OnwMin(0)] public int TopPlantsSubtractByDay {get; private set; } = 5;
        [field: SerializeField, DisplayAs("재난 확률 고정 수치"), OnwMin(0)] public int TopFixationDisasterProbability {get; private set; } = 10;
        
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;

        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "PlantsSubtractByDay", TopPlantsSubtractByDay },
            { "FixationDisasterProbability", TopFixationDisasterProbability }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
        protected override void TriggerTopEvent()
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            
            void onChangedDay(int day)
            {
                TMPlayerManager.Instance.Plants.Value -= TopPlantsSubtractByDay; 
            }
        }
        
        protected override void TriggerBottomEvent()
        {
        }
    }
}