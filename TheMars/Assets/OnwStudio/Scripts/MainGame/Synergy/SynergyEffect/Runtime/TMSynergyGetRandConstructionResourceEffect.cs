using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TM.Manager;
using TM.Synergy.Effect.Creator;
using UnityEngine.Localization;

namespace TM.Synergy.Effect
{
    [System.Serializable]
    public sealed class TMSynergyGetRandConstructionResourceEffect : TMSynergyEffect, ITMSynergyInitializeEffect<TMSynergyGetRandConstructionResourceEffectCreator>
    {
        [SerializeField, ReadOnly] private LocalizedString _localizedDescription = new("TM_Synergy_Effect", "Get_Rand_Construction_Resource_Effect");
        
        public override LocalizedString LocalizedDescription => _localizedDescription;

        [field: SerializeField, ReadOnly] public int Resource { get; private set; } = 10;
        [field: SerializeField, ReadOnly] public int RepeatDay { get; private set; }

        private int _dayCount = -1;
        
        public void Initialize(TMSynergyGetRandConstructionResourceEffectCreator effectCreator)
        {
            Resource = effectCreator.Resource;
            RepeatDay = effectCreator.RepeatDay;
            
            _localizedDescription.Arguments = new object[]
            {
                new
                {
                    Resource = Mathf.Abs(Resource),
                    RepeatDay,
                    RemainingDay = RepeatDay - _dayCount,
                    IsActive = false,
                    Positive = Resource >= 0
                }
            };
        }
        
        public override void ApplyEffect(TMSynergy synergy)
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
        }
        
        public override void UnapplyEffect(TMSynergy synergy)
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            _dayCount = 0;
            
            _localizedDescription.Arguments = new object[]
            {
                new
                {
                    Resource = Mathf.Abs(Resource),
                    RepeatDay,
                    RemainingDay = 0,
                    IsActive = true,
                    Positive = Resource >= 0
                }
            };
        }

        private void onChangedDay(int day)
        {
            _dayCount++;
            if (_dayCount >= RepeatDay)
            {
                _dayCount = 0;
                
                int rand = Random.Range(0, 3);

                switch (rand)
                {
                    case 0:
                        TMPlayerManager.Instance.Steel.Value += Resource;
                        break;
                    case 1:
                        TMPlayerManager.Instance.Plants.Value += Resource;
                        break;
                    case 2:
                        TMPlayerManager.Instance.Clay.Value += Resource;
                        break;
                }
            }
            
            _localizedDescription.Arguments = new object[]
            {
                new
                {
                    Resource = Mathf.Abs(Resource),
                    RepeatDay,
                    RemainingDay = RepeatDay - _dayCount,
                    IsActive = true,
                    Positive = Resource >= 0
                }
            };
        }
    }
}