using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Synergy.Effect.Creator;

namespace TM.Synergy.Effect
{
    public interface ITMSynergyInitializeEffect<in T> where T : TMSynergyEffectCreator
    {
        void Initialize(T effectCreator);
    }
}
