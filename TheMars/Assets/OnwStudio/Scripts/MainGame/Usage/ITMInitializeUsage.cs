using System.Collections;
using System.Collections.Generic;
using TM.Usage.Creator;
using UnityEngine;

namespace TM.Usage
{
    public interface ITMInitializeUsage<in T> where T : ITMUsageCreator
    {
        void Initialize(T creator);
    }
}
