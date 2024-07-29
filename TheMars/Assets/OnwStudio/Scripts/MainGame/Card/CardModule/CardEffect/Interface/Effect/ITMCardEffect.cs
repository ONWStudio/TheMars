using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect
{

    public interface ITMCardEffect
    {
        /// <summary>
        /// .. 카드 사용시
        /// </summary>
        void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger);
    }

    public interface ITMInitializableEffect<T> where T : ITMEffectCreator
    {
        void Initialize(T effectCreator);
    }
}