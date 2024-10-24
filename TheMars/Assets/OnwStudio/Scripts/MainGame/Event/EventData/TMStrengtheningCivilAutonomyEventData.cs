using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event
{
    public sealed class TMStrengtheningCivilAutonomyEventData : TMEventData
    {
        public override bool CanFireTop =>
            TMPlayerManager.Instance.Credit >= TopCreditSubtract &&
            TMPlayerManager.Instance.Satisfaction >= TopSatisfactionAdd;

        public override bool CanFireBottom => true;
        
        public override List<object> TopEffectLocalizedArguments => new()
        {
            TopTargetBuildingCount, 
            TopStopDayCount, 
            TopCreditSubtract, 
            TopSatisfactionAdd
        };
        
        public override List<object> BottomEffectLocalizedArguments => new()
        {
            BottomSatisfactionSubtract,
            BottomMarsLithiumPaymentEventSubtract
        };

        [field: InfoBox("효과 고유값 Key \n" +
                        "정지할 건물 개수 : TargetBuildingCount \n" +
                        "건물 정지 일수 : StopDayCount \n" +
                        "마르스 리튬 지급량 감소 : MarsLithiumPaymentEventSubtract")]
        [field: Space(10)]
        [field: Header("내가 이걸 하기에는 너무 많은 일이야... 차라리 하위 조직을 만들어 볼까? 선택지")]
        [field: SerializeField, Min(0)] public int TopTargetBuildingCount { get; private set; } = 2;
        [field: SerializeField, Min(1)] public int TopStopDayCount { get; private set; } = 1;
        [field: SerializeField, Min(0)] public int TopCreditSubtract { get; private set; } = 40;
        [field: SerializeField, Min(0)] public int TopSatisfactionAdd { get; private set; } = 10;
        
        [field: Header("정부의 생각이 맞다. 지금은 효율이 중요해.")]
        [field: SerializeField, Min(0)] public int BottomSatisfactionSubtract { get; private set; } = 10;
        [field: SerializeField, Min(0)] public int BottomMarsLithiumPaymentEventSubtract { get; private set; } = 10;
        
        protected override void TriggerLeftEvent()
        {
            TMPlayerManager.Instance.Credit -= TopCreditSubtract;
            TMPlayerManager.Instance.Satisfaction += TopSatisfactionAdd;
        }
        
        protected override void TriggerRightEvent()
        {
            TMPlayerManager.Instance.Satisfaction -= BottomSatisfactionSubtract;
        }
    }
}
