using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Interface;
using TM.Manager;
using TM.Synergy.Effect.Creator;

namespace TM.Synergy.Effect
{
    public abstract class TMSynergyEffect : IDescriptable
    {
        public int TargetBuildingCount => Creator?.TargetBuildingCount ?? 0;
        public abstract string Description { get; }

        public TMSynergyEffectCreator Creator { get; init; } = null;
        
        /// <summary>
        /// .. TargetCount가 충족되었을때 효과 적용
        /// </summary>
        public abstract void ApplyEffect();
        public abstract void UnapplyEffect();
    }
}
