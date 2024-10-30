using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event
{
    [Substitution("기업 주도 체제")]
    public sealed class TMCorporateDrivenSystemEventData : TMEventData
    {
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;

        [field: InfoBox("효과 고유값 key \n" +
                        "카드 획득 이벤트 시 선택 카드량 감소 카운트 : CollectCardCountSubtract \n" +
                        "카드 획득 이벤트 시 선택 카드량 감소 지속일수 : CollectCardCountDayCount")]
        [field: Header("시장의 자유 경제만 유지해 준다면 건들지는 말자 선택지")]
        [field: SerializeField, DisplayAs("카드 획득 이벤트 시 선택 카드량 감소 카운트"), OnwMin(0)] public int TopCollectCardCountSubtract { get; private set; } = 1;
        [field: SerializeField, DisplayAs("카드 획득 이벤트 시 선택 카드량 감소 지속일수"), OnwMin(0)] public int TopCollectCardCountDayCount { get; private set; } = 4;
        [field: SerializeField, DisplayAs("획득 인구 수"), OnwMin(0)] public int TopPopulationAdd { get; private set; } = 5;

        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "CollectCardCountSubtract", TopCollectCardCountSubtract },
            { "CollectCardCountDayCount", TopCollectCardCountDayCount },
            { "PopulationAdd", TopPopulationAdd }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;

        protected override void TriggerTopEvent()
        {
            TMPlayerManager.Instance.Population += TopPopulationAdd;
        }

        protected override void TriggerBottomEvent()
        {
        }
    }
}