using System.Collections;
using System.Collections.Generic;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 획득")]
    public sealed class TMCardCollectEffectCreator : ITMNormalEffectCreator
    {
        [field: SerializeField, Min(1)] public int CollectCount { get; private set; } = 1;

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardCollectEffect, TMCardCollectEffectCreator>(this);
        }
    }
}
