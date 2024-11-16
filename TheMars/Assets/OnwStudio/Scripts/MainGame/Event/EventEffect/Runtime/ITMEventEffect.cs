using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Event.Effect
{
    public interface ITMEventEffect
    {
        LocalizedString EffectDescription { get; }

        void ApplyEffect();
    }
}
