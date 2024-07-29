using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    public interface ITMEffectTrigger
    {
        CardEvent OnEffectEvent { get; }
    }
}
