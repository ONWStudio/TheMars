using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    [Substitution("진보와 균형 ")]
    public sealed class TMProgressAndBalanceEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 무작위 자원 획득량 : RandResourceAddByDay \n" +
                        "\t 무작위 자원 획득 지속일수 : RandResourceAddDayCount")]
        [field: Header("과학의 발전에 이바지 한다. 선택지")]
        [field: SerializeField, DisplayAs("무작위 자원 획득량"), OnwMin(0)] public int TopRandResourceAddByDay { get; private set; } = 20;
        [field: SerializeField, DisplayAs("무작위 자원 획득 지속일수"), OnwMin(0)] public int TopRandResourceAddDayCount { get; private set; } = 3;

        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 무작위로 선택할 건물 개수 : TargetBuildingCount")]
        [field: Header("화성의 자연 또한 자연이다.")]
        [field: SerializeField, DisplayAs("무작위로 선택할 건물 개수"), OnwMin(0)] public int BottomTargetBuildingCount { get; private set; }

        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;

        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            {
                "RandResourceAddByDay", TopRandResourceAddByDay
            },
            {
                "RandResourceAddDayCount", TopRandResourceAddDayCount
            }
        };

        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
        {
            {
                "TargetBuildingCount", BottomTargetBuildingCount
            }
        };

        protected override void TriggerTopEvent()
        {
            int dayCount = 0;
            TMSimulator.Instance.OnChangedDay += onChangedDay;

            void onChangedDay(int day)
            {
                dayCount++;

                if (dayCount == TopRandResourceAddDayCount)
                {
                    TMSimulator.Instance.OnChangedDay -= onChangedDay;
                }

                switch (Random.Range(0, 3))
                {
                    case 0:
                        TMPlayerManager.Instance.Steel += TopRandResourceAddByDay;
                        break;
                    case 1:
                        TMPlayerManager.Instance.Plants += TopRandResourceAddByDay;
                        break;
                    default:
                        TMPlayerManager.Instance.Clay += TopRandResourceAddByDay;
                        break;
                }
            }
        }

        protected override void TriggerBottomEvent()
        {
        }
    }
}