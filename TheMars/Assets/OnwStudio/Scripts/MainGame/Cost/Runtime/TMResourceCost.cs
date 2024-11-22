using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Cost.Creator;
using Onw.Event;

namespace TM.Cost
{
    [System.Serializable]
    public class TMResourceCost : ITMResourceCost, ITMInitializeCost<TMResourceCostCreator>
    {
        [SerializeField] private ReactiveField<int> _additionalCost = new();

        [field: SerializeField, ReadOnly] public LocalizedString LocalizedDescription { get; private set; } = new("TM_Cost", "Resource_Cost");
        [field: SerializeField, ReadOnly] public TMResourceKind Kind { get; private set; }
        [field: SerializeField, ReadOnly] public int Cost { get; private set; }

        public IReactiveField<int> AdditionalCost => _additionalCost;

        public bool CanProcessPayment => TMPlayerManager.Instance.GetResourceByKind(Kind) >= FinalCost;
        public int FinalCost => Cost + AdditionalCost.Value;

        public void Initialize(TMResourceCostCreator creator)
        {
            Kind = creator.Kind;
            Cost = creator.Cost;

            setArguments();

            _additionalCost.AddListener(onChangedAdditionalCost);

            void onChangedAdditionalCost(int additionalCost)
            {
                setArguments();
            }

            void setArguments()
            {
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
            TMPlayerManager.Instance.AddResource(Kind, -Cost);
        }
    }
}

