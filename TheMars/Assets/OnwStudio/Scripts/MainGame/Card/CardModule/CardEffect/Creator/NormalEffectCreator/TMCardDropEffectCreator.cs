using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("카드 버리기")]
    public sealed class TMCardDropEffectCreator : ITMNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardDropEffect, TMCardDropEffectCreator>(this);
        }
    }
}
