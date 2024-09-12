using System.Collections;
using System.Collections.Generic;
using Onw.Interface;
using UnityEngine;

namespace TM.Synergy.Effect
{
    public abstract class TMSynergyEffect : IDescriptable
    {
        [field: SerializeField] public int TargetBuildingCount { get; private set; }
        public abstract string Description { get; }

        public abstract void ApplyEffect();
    }
}
