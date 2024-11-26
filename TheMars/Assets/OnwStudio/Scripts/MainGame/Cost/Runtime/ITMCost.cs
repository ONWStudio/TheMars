using Onw.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Cost
{
    public interface ITMResourceCost : ITMCost
    {
        TMResourceKind Kind { get; }
        int Cost { get; }
        int FinalCost { get; }
        IReactiveField<int> AdditionalCost { get; }
    }

    public interface ITMCost
    {
        bool CanProcessPayment { get; }
        LocalizedString CostDescription { get; } 
        void ApplyCosts();
    }
}
