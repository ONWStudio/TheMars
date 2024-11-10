//using System.Collections;
//using System.Collections.Generic;
//using Onw.Attribute;
//using TM.Manager;
//using UnityEngine;

//namespace TM.Event
//{
//    [Substitution("민주적 자치, 그 다음은?")]
//    public sealed class TMDemocraticAutonomyNextStepsEventData : TMEventData
//    {
//        [field: InfoBox("고유 효과값 Key \n \n" +
//                        "\t 만족도 : SatisfactionAddByDay \n" +
//                        "\t 만족도 : SatisfactionAddDayCount \n" +
//                        "\t 크레딧 : CreditAddByDay \n" +
//                        "\t 크레딧 : CreditAddDayCount \n")]
//        [field: Header("시민의 의견을 전적으로 들어야 된다. 선택지")]
//        [field: SerializeField, DisplayAs("만족도 획득량"), OnwMin(0)] public int TopSatisfactionAddByDay { get; private set; } = 20;
//        [field: SerializeField, DisplayAs("만족도 획득 지속일수"), OnwMin(0)] public int TopSatisfactionAddDayCount { get; private set; } = 1;
//        [field: SerializeField, DisplayAs("크레딧 획득량"), OnwMin(0)] public int TopCreditAddByDay { get; private set; } = 10;
//        [field: SerializeField, DisplayAs("크레딧 획득 지속일수"), OnwMin(0)] public int TopCreditAddDayCount { get; private set; } = 5;

//        [field: InfoBox("고유 효과값 Key \n \n" +
//                        "\t 만족도 감소량 : SatisfactionSubtractByDay \n" +
//                        "\t 만족도 감소 지속일수 : SatisfactionSubtractDayCount \n" +
//                        "\t 마르스 리튬 납부 이벤트 납부량 감소 : MarsLithiumEventSubtract\n")]
//        [field: Header("균형을 유지 해야 된다. 선택지")]
//        [field: SerializeField, DisplayAs("만족도 감소량"), OnwMin(0)] public int BottomSatisfactionSubtractByDay { get; private set; } = 2;
//        [field: SerializeField, DisplayAs("만족도 감소 지속일수"), OnwMin(0)] public int BottomSatisfactionSubtractDayCount { get; private set; } = 3;
//        [field: SerializeField, DisplayAs("마르스 리튬 납부 이벤트 납부량 감소"), OnwMin(0)] public int BottomMarsLithiumEventSubtract { get; private set; } = 40;

//        public override bool CanFireTop => true;
//        public override bool CanFireBottom => true;

//        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
//        {
//            {
//                "SatisfactionAddByDay", TopSatisfactionAddByDay
//            },
//            {
//                "SatisfactionAddDayCount", TopSatisfactionAddDayCount
//            },
//            {
//                "CreditAddByDay", TopCreditAddByDay
//            },
//            {
//                "CreditAddDayCount", TopCreditAddDayCount
//            }
//        };

//        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
//        {
//            {
//                "SatisfactionSubtractByDay", BottomSatisfactionSubtractByDay
//            },
//            {
//                "SatisfactionSubtractDayCount", BottomSatisfactionSubtractDayCount
//            },
//            {
//                "MarsLithiumEventSubtract", BottomMarsLithiumEventSubtract
//            }
//        };

//        protected override void TriggerTopEvent()
//        {
//            int dayCountBySatisfaction = 0;
//            int dayCountByCredit = 0;

//            TMSimulator.Instance.NowDay.AddListener(onChangedDayBySatisfaction);
//            TMSimulator.Instance.NowDay.AddListener(onChangedDayByCredit);

//            void onChangedDayBySatisfaction(int day)
//            {
//                dayCountBySatisfaction++;

//                if (dayCountBySatisfaction == TopSatisfactionAddDayCount)
//                {
//                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDayBySatisfaction);
//                }

//                TMPlayerManager.Instance.Satisfaction.Value += TopSatisfactionAddByDay;
//            }

//            void onChangedDayByCredit(int day)
//            {
//                dayCountByCredit++;

//                if (dayCountByCredit == TopCreditAddDayCount)
//                {
//                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDayByCredit);
//                }

//                TMPlayerManager.Instance.Credit.Value += TopCreditAddByDay;
//            }
//        }
//        protected override void TriggerBottomEvent()
//        {
//            int dayCount = 0;

//            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

//            void onChangedDay(int day)
//            {
//                dayCount++;

//                if (dayCount == BottomSatisfactionSubtractDayCount)
//                {
//                    TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
//                }

//                TMPlayerManager.Instance.Satisfaction.Value -= BottomSatisfactionSubtractByDay;
//            }
//        }
//    }
//}