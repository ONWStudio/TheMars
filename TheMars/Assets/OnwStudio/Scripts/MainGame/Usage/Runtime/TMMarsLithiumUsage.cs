using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Event;
using TM.Manager;
using TM.Usage.Creator;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Usage
{
    [System.Serializable]
    public class TMMarsLithiumUsage : ITMUsage, ITMInitializeUsage<TMMarsLithiumUsageCreator>
    {

        [field: SerializeField, ReadOnly]private int _nowDay = 0;

        public bool CanProcessPayment => MarsLithium * _nowDay <= TMPlayerManager.Instance.MarsLithium.Value;

        [field: SerializeField, ReadOnly] public LocalizedString UsageLocalizedString { get; private set; } = new("TM_Cost", "MarsLithium_Usage");

        [field: SerializeField] public int MarsLithium { get; set; }

        public void Initialize(TMMarsLithiumUsageCreator creator)
        {
            TMSimulator.Instance.NowDay.AddListener(onChagendDay);
            TMEventManager.Instance.MarsLithiumEventAddResource.AddListener(onChangedAdditionalMarsLithium);

            void onChagendDay(int day)
            {
                UsageLocalizedString.Arguments = new object[]
                {
                    new
                    {
                        MarsLithium = MarsLithium * _nowDay
                    }
                };

                _nowDay = day;
            }

            void onChangedAdditionalMarsLithium(int additionalMarsLithium)
            {
                MarsLithium += additionalMarsLithium;
            }
        }

        public void ApplyUsage()
        {
            TMPlayerManager.Instance.MarsLithium.Value -= MarsLithium * _nowDay;
        }
    }
}
