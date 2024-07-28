using System.Collections;
using System.Collections.Generic;
using TMCard.Runtime;
using UnityEngine;

namespace TMCard.Effect
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("Ä«µå È¹µæ")]
    public sealed class TMCardCollectEffectCreator : ITMNormalEffectCreator
    {
        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TMCardCollectEffect, TMCardCollectEffectCreator>(this);
        }
    }
}
