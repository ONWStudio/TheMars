using System.Collections;
using System.Collections.Generic;
using TMCard.Effect;
using UnityEngine;

namespace TMCard.Effect
{
    public interface ITMInitializableEffect<T> where T : ITMEffectCreator
    {
        void Initialize(T effectCreator);
    }
}