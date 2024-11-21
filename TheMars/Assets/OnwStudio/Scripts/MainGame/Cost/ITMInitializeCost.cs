using System.Collections;
using System.Collections.Generic;
using TM.Cost.Creator;
using UnityEngine;

namespace TM.Cost
{
    public interface ITMInitializeCost<in T> where T : ITMCostCreator
    {
        void Initialize(T creator);
    }
}
