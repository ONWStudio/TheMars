using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Usage
{
    public interface ITMUsage
    {
        bool CanProcessPayment { get; }
        LocalizedString UsageLocalizedString { get; } 
        void ApplyUsage();
    }
}
