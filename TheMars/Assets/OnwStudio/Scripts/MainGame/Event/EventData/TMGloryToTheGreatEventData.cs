using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;

namespace TM.Event
{
    [Substitution("위대한 자들을 찬양하리라")]
    public sealed class TMGloryToTheGreatEventData : TMEventData
    {
        [field: InfoBox("효과 고유값 Key \n \n" +
                        "\t 건물 중 무작위 등급업시킬 건물개수 : TargetBuildingCount")]
        [field: Header("선택지 1")]
        [field: SerializeField, DisplayAs("건물 중 무작위 등급업시킬 건물개수"), OnwMin(0)] public int TopTargetBuildingCount { get; private set; } = 3;
        
        public override bool CanFireTop => true;
        public override bool CanFireBottom => true;
        
        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
        {
            { "TargetBuildingCount", TopTargetBuildingCount }
        };
        
        public override Dictionary<string, object> BottomEffectLocalizedArguments => null;
        
        protected override void TriggerTopEvent()
        {
        }
        
        protected override void TriggerBottomEvent()
        {
        }
    }
}