using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Manager;

namespace TM.Usage.Creator
{
    [SerializeField, SerializeReferenceDropdownName("마르스 리튬 소모 (n일차의 배수)")]
    public class TMMarsLithiumUsageCreator : ITMUsageCreator
    {
        [field: SerializeField, DisplayAs("기본 납부량")] public int MarsLithium { get; private set; } = 10;

        public ITMUsage CreateUsage()
        {
            return ITMUsageCreator.CreateEffect<TMMarsLithiumUsage, TMMarsLithiumUsageCreator>(this);
        }
    }
}