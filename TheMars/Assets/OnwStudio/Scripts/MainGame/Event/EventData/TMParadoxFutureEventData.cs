using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [Substitution("파라독스 퓨처")]
    public class TMParadoxFutureEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 만족도 감소량(하루마다) : SatisfactionSubtractByDay \n" +
                        "\t 크레딧 감소량(하루마다) : SatisfactionSubtractDayCount \n" +
                        "\t 건물 건설 시 추가 소모 되는 자원량 : ResourceSubtractByCreateBuilding")]
        [field: Header("재난으로 일어난 피해를 줄여야 된다... 선택지")]
        [field: SerializeField, DisplayAs("만족도 감소량(하루마다)"), Min(0)] public int TopSatisfactionSubtractByDay { get; private set; } = 10;
        [field: SerializeField, DisplayAs("크레딧 감소량(하루마다)"), Min(0)] public int TopCreditSubtractByDay { get; private set; } = 40;
        [field: SerializeField, DisplayAs("건물 건설 시 추가 소모 되는 자원량"), Min(0)] public int TopResourceSubtractByCreateBuilding { get; private set; } = 20;
        
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionSubtractByDay", TopSatisfactionSubtractByDay},
            { "CreditSubtractByDay", TopCreditSubtractByDay },
            { "ResourceSubtractByCreateBuilding", TopResourceSubtractByCreateBuilding }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
        protected override void TriggerTopEvent()
        {
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            
            void onChangedDay(int day)
            {
                TMPlayerManager.Instance.Satisfaction -= TopSatisfactionSubtractByDay;
                TMPlayerManager.Instance.Credit -= TopCreditSubtractByDay;
            }
        }
        
        protected override void TriggerBottomEvent()
        {
        }
    }
}
