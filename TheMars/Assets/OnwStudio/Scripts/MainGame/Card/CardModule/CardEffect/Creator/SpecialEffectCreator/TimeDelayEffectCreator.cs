using Onw.Attribute;
using UnityEngine;
namespace TMCard.Effect
{
    using static ITMCardEffectCreator;

    [SerializeReferenceDropdownName("(특수) 소요 (시간)"), Substitution("소요(시간)")]
    public class TimeDelayEffectCreator : ITMCardSpecialEffectCreator
    {
        [field: SerializeField, DisplayAs("딜레이 시간 (초)")] public float DelayTime { get; private set; } = 1f;

        public ITMCardEffect CreateEffect()
        {
            return CardEffectGenerator.CreateEffect<TimeDelayEffect, TimeDelayEffectCreator>(this);
        }
    }
}
