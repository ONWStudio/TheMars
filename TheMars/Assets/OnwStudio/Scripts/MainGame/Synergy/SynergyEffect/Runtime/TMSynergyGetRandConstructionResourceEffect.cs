using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TM.Manager;
using TM.Synergy.Effect.Creator;

namespace TM.Synergy.Effect
{
    [System.Serializable]
    public sealed class TMSynergyGetRandConstructionResourceEffect : TMSynergyEffect, ITMSynergyInitializeEffect<TMSynergyGetRandConstructionResourceEffectCreator>
    {
        public override string Description => $"({TargetBuildingCount}) : 하루가 지날 때 마다 건설 자원(강철, 식물, 점토)을 무작위로 +{Resource}개 획득한다";

        [field: SerializeField, ReadOnly] public int Resource { get; private set; } = 10;

        public override void ApplyEffect(TMSynergy synergy)
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
        }
        
        public override void UnapplyEffect(TMSynergy synergy)
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
        }

        private void onChangedDay(int day)
        {
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
        
        public void Initialize(TMSynergyGetRandConstructionResourceEffectCreator effectCreator)
        {
            Resource = effectCreator.Resource;
        }
    }
}