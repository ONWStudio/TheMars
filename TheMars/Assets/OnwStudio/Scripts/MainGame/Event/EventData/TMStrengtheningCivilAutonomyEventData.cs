using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event
{
    [Substitution("시민 자치 강화")]
    public sealed class TMStrengtheningCivilAutonomyEventData : TMEventData
    {
        public override bool CanFireTop =>
            TMPlayerManager.Instance.Credit >= TopCreditSubtract &&
            TMPlayerManager.Instance.Satisfaction >= TopSatisfactionAdd;

        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "TargetBuildingCount", TopTargetBuildingCount }, 
            { "StopDayCount", TopStopDayCount }, 
            { "CreditSubtract", TopCreditSubtract }, 
            { "SatisfactionAdd", TopSatisfactionAdd }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
        {
            { "SatisfactionSubtract", BottomSatisfactionSubtract },
            { "MarsLithiumPaymentEventSubtract", BottomMarsLithiumPaymentEventSubtract }
        };

        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 정지할 건물 개수 : TargetBuildingCount \n" +
                        "\t 건물 정지 일수 : StopDayCount")]
        [field: Space(10)]
        [field: Header("내가 이걸 하기에는 너무 많은 일이야... 차라리 하위 조직을 만들어 볼까? 선택지")]
        [field: SerializeField, DisplayAs("가동중지 할 건물 개수"), OnwMin(0)] public int TopTargetBuildingCount { get; private set; } = 2;
        [field: SerializeField, DisplayAs("가동중지 지속 일수"), OnwMin(1)] public int TopStopDayCount { get; private set; } = 1;
        [field: SerializeField, DisplayAs("크레딧 소모량"), OnwMin(0)] public int TopCreditSubtract { get; private set; } = 40;
        [field: SerializeField, DisplayAs("만족도 획득량"), OnwMin(0)] public int TopSatisfactionAdd { get; private set; } = 10;
        
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 납부 이벤트 시 요구 마르스리튬 감소량 : MarsLithiumPaymentEventSubtract")]
        [field: Header("정부의 생각이 맞다. 지금은 효율이 중요해. 선택지")]
        [field: SerializeField, DisplayAs("만족도 감소량"), OnwMin(0)] public int BottomSatisfactionSubtract { get; private set; } = 10;
        [field: SerializeField, DisplayAs("\"마르스 리튬\" 납부 이벤트 시 요구 마르스리튬 감소량"), OnwMin(0)] public int BottomMarsLithiumPaymentEventSubtract { get; private set; } = 10;
        
        protected override void TriggerTopEvent()
        {
            TMPlayerManager.Instance.Credit -= TopCreditSubtract;
            TMPlayerManager.Instance.Satisfaction += TopSatisfactionAdd;
        }
        
        protected override void TriggerBottomEvent()
        {
            TMPlayerManager.Instance.Satisfaction -= BottomSatisfactionSubtract;
        }
    }
}
