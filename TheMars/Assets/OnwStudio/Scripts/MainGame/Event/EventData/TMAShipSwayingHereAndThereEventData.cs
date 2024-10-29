using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [SuppressMessage("ReSharper", "InconsistentNaming"), Substitution("이리저리 흔들리는 배")]
    public sealed class TMAShipSwayingHereAndThereEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 만족도 획득량 : SatisfactionAddByDay \n" +
                        "\t 만족도 획득 지속일수 : SatisfactionAddDayCount \n" +
                        "\t 크레딧 획득량 : CreditAddByDay \n" +
                        "\t 크레딧 획득 지속일수 : CreditAddDayCount \n")]
        [field: Header("나는 그들이 원하는 것을 들어주고 있을 뿐이다. 선택지")]
        [field: SerializeField, DisplayAs("만족도 획득량"), OnwMin(0)] public int TopSatisfactionAddByDay { get; private set; } = 5;
        [field: SerializeField, DisplayAs("만족도 획득 지속일수"), OnwMin(0)] public int TopSatisfactionAddDayCount { get; private set; } = 3;
        [field: SerializeField, DisplayAs("크레딧 획득량"), OnwMin(0)] public int TopCreditAddByDay { get; private set; } = 30;
        [field: SerializeField, DisplayAs("크레딧 획득 지속일수"), OnwMin(0)] public int TopCreditAddDayCount { get; private set; } = 2;

        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 만족도 획득량 : SatisfactionAddByDay \n" +
                        "\t 만족도 획득 지속일수 : SatisfactionAddDayCount \n")]
        [field: Header("이제 와서 바꾸기는 어렵다. 다른이에게 이 일을 맡겨야겠다. 선택지")]
        [field: SerializeField, DisplayAs("만족도 획득량"), OnwMin(0)] public int BottomSatisfactionAddByDay { get; private set; } = 5;
        [field: SerializeField, DisplayAs("만족도 획득 지속일수"), OnwMin(0)] public int BottomSatisfactionAddDayCount { get; private set; } = 2;

        public override bool CanFireTop => true;

        public override bool CanFireBottom => true;

        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "SatisfactionAddByDay", TopSatisfactionAddByDay },
            { "SatisfactionAddDayCount", TopSatisfactionAddDayCount },
            { "CreditAddByDay", TopCreditAddByDay },
            { "CreditAddDayCount", TopCreditAddDayCount }
        };

        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
        {
            { "SatisfactionAddByDay", BottomSatisfactionAddByDay },
            { "SatisfactionAdd", BottomSatisfactionAddDayCount }
        };
        
        protected override void TriggerTopEvent()
        {
            int dayCountBySatisfaction = 0;
            int dayCountByCredit = 0;

            TMSimulator.Instance.OnChangedDay += onChangedDayBySatisfaction;
            TMSimulator.Instance.OnChangedDay += onChangedDayByCredit;

            void onChangedDayBySatisfaction(int day)
            {
                dayCountBySatisfaction++;

                if (dayCountBySatisfaction == TopSatisfactionAddDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDayBySatisfaction;
                }

                TMPlayerManager.Instance.Satisfaction += TopSatisfactionAddByDay;
            }

            void onChangedDayByCredit(int day)
            {
                dayCountByCredit++;

                if (dayCountByCredit == TopCreditAddDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDayByCredit;
                }
                
                TMPlayerManager.Instance.Credit += TopCreditAddByDay;
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
                
                TMPlayerManager.Instance.Satisfaction += BottomSatisfactionAddByDay;
            }
        }
    }
}