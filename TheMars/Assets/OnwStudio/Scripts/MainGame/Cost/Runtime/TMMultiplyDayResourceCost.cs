using JetBrains.Annotations;
using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Event;
using TM.Manager;
using TM.Cost.Creator;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Event;

namespace TM.Cost
{
    [System.Serializable]
    public class TMMultiplyDayResourceCost : ITMResourceCost, ITMInitializeCost<TMMultiplyDayResourceCostCreator>
    {

        [SerializeField, ReadOnly] private int _nowDay = 0;
        [SerializeField, ReadOnly] private ReactiveField<int> _additionalCost = new();

        public bool CanProcessPayment => FinalCost <= TMPlayerManager.Instance.MarsLithium.Value;

        [field: SerializeField, ReadOnly] public LocalizedString LocalizedDescription { get; private set; } = new("TM_Cost", "Resource_Cost");

        [field: SerializeField, ReadOnly] public TMResourceKind Kind { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }

        public int FinalCost => Cost * _nowDay;

        public IReactiveField<int> AdditionalCost => _additionalCost;

        public void Initialize(TMMultiplyDayResourceCostCreator creator)
        {
            Kind = creator.Kind;
            Cost = creator.Cost;
            TMSimulator.Instance.NowDay.AddListener(onChagendDay);

            void onChagendDay(int day)
            {
                _nowDay = day;

                LocalizedDescription.Arguments = new object[]
                {
                    new
                    {
                        Kind,
                        Cost = FinalCost
                    }
                };
            }
        }

        public void ApplyCosts()
        {
            TMPlayerManager.Instance.MarsLithium.Value -= FinalCost;
        }
    }
}
