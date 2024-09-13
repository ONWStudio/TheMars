using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Interface;
using Onw.ServiceLocator;
using TM.Manager;
using UnityEngine;

namespace TM.Synergy.Effect
{
    public abstract class TMSynergyEffect : IDescriptable
    {
        [field: SerializeField, DisplayAs("시너지 건물 개수")] public int TargetBuildingCount { get; private set; }
        public abstract string Description { get; }

        /// <summary>
        /// .. TargetCount가 충족되었을때 효과 적용
        /// </summary>
        public abstract void ApplyEffect();
        public abstract void UnapplyEffect();
    }
    
    public class TMSynergyGetRandConstructionResourceEffect : TMSynergyEffect
    {
        public override string Description => $"({TargetBuildingCount}) : 하루가 지날 때 마다 건설 자원(강철, 식물, 점토)을 무작위로 +{Resource}개 획득한다";

        [field: SerializeField, DisplayAs("획득할 건설 자원 개수")] public int Resource { get; private set; } = 10;
        
        public override void ApplyEffect()
        {
            ServiceLocator<TMSimulator>.InvokeService(simulator => simulator.OnChangedDay += onChangedDay);
        }
        
        public override void UnapplyEffect()
        {
            ServiceLocator<TMSimulator>.InvokeService(simulator => simulator.OnChangedDay -= onChangedDay);
        }

        private void onChangedDay(int day)
        {
            if (!ServiceLocator<PlayerManager>.TryGetService(out PlayerManager player)) return;

            int rand = Random.Range(0, 3);

            switch (rand)
            {
                case 0:
                    player.Steel += Resource;
                    break;
                case 1:
                    player.Plants += Resource;
                    break;
                case 2:
                    player.Clay += Resource;
                    break;
            }
        }
    }
}
