using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect.Creator;
using UnityEngine;

namespace TM.Event.Effect
{
    public interface ITMEventInitializeEffect<in T> where T : ITMEventEffectCreator
    {
        void Initialize(T creator);
    }
}
